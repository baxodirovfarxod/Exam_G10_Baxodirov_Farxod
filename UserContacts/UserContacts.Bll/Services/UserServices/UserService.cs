using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContacts.Bll.Dtos;
using UserContacts.Core.Errors;
using UserContacts.Dal;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.UserServices;

public class UserService : IUserService
{
    private readonly ILogger<UserService> logger;
    private readonly MainContext mainContext;
    private readonly IMapper mapper;

    public UserService(MainContext mainContext, IMapper mapper, ILogger<UserService> logger)
    {
        this.mainContext = mainContext;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<long> AddUserAync(UserCreateDto user)
    {
        var userEntity = mapper.Map<User>(user);
        await mainContext.Users.AddAsync(userEntity);
        await mainContext.SaveChangesAsync();
        return userEntity.UserId;
    }

    public async Task<bool> CheckEmailExists(string email)
    {
        return await mainContext.Users
                                .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> CheckUserById(long userId)
    {
        return await mainContext.Users
                                .AnyAsync(u => u.UserId == userId);
    }

    public async Task<bool> CheckUsernameExists(string username)
    {
        return await mainContext.Users
                                .AnyAsync(u => u.Username == username);
    }

    public async Task DeleteUserByIdAsync(long userId)
    {
        var user = await mainContext.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            throw new EntityNotFoundException();
        }

       logger.LogInformation("Deleting user with ID: {UserId}", userId);

        mainContext.Users.Remove(user);
        await mainContext.SaveChangesAsync();
    }

    public async Task<UserGetDto> GetUserByIdAync(long id)
    {
        var user = await mainContext.Users
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        var userDto = mapper.Map<UserGetDto>(user);
        return userDto;
    }

    public async Task<User> GetUserByUsernameAync(string userName)
    {
        var user = await mainContext.Users
            .FirstOrDefaultAsync(u => u.Username == userName);

        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole)
    {
        var user = await GetUserByIdAync(userId);

        user.Role = userRole;
        mainContext.Users.Update(mapper.Map<User>(user));
        await mainContext.SaveChangesAsync();
    }
}
