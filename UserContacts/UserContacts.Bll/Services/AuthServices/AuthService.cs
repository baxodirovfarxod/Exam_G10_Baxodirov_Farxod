using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using UserContacts.Bll.Dtos;
using UserContacts.Bll.Helpers;
using UserContacts.Bll.Helpers.Security;
using UserContacts.Bll.Services.RefreshTokenService;
using UserContacts.Bll.Services.RoleRepositories;
using UserContacts.Bll.Services.UserServices;
using UserContacts.Bll.Settings;
using UserContacts.Core.Errors;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> logger;
    private readonly IUserService userService;
    private readonly ITokenService tokenService;
    private readonly JwtAppSettings jwtAppSettings;
    private readonly IRefreshTokenService refreshTokenService;
    private readonly IRoleService roleService;
    private readonly int Expires;
    private readonly IMapper mapper;

    public AuthService(IUserService userService, ITokenService tokenService, JwtAppSettings jwtAppSettings, IRefreshTokenService refreshTokenService, IRoleService roleService, IMapper mapper)
    {
        this.userService = userService;
        this.tokenService = tokenService;
        this.jwtAppSettings = jwtAppSettings;
        this.refreshTokenService = refreshTokenService;
        this.roleService = roleService;
        Expires = int.Parse(jwtAppSettings.Lifetime);
        this.mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        logger.LogInformation("Login user with username: {UserName}", userLoginDto.UserName);
        var user = await userService.GetUserByUsernameAync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }

        var userGetDto = mapper.Map<UserGetDto>(user);

        var token = tokenService.GenerateToken(userGetDto);
        var refreshToken = tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await refreshTokenService.AddRefreshToken(refreshTokenToDB);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24
        };

        return loginResponseDto;
    }

    public async Task LogOut(string token)
    {
        await refreshTokenService.DeleteRefreshToken(token);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await refreshTokenService.GetRefreshToken(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await userService.GetUserByIdAync(userId);

        var userGetDto = mapper.Map<UserGetDto>(user);

        var newAccessToken = tokenService.GenerateToken(userGetDto);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await refreshTokenService.AddRefreshToken(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }

    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);
        var user = mapper.Map<User>(userCreateDto);
        user.Password = tupleFromHasher.Hash;
        user.Salt = tupleFromHasher.Salt;
        user.UserRoleId = await roleService.GetRoleIdAsync("User");

        return await userService.AddUserAync(mapper.Map<UserCreateDto>(user));
    }
}
