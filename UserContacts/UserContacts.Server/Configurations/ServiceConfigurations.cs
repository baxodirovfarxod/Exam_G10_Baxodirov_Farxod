using UserContacts.Bll.MappingProfile;
using UserContacts.Bll.Services.AuthServices;
using UserContacts.Bll.Services.ContactServices;
using UserContacts.Bll.Services.RefreshTokenService;
using UserContacts.Bll.Services.RoleRepositories;
using UserContacts.Bll.Services.UserServices;

namespace UserContacts.Server.Configurations;

public static class ServiceConfigurations
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IContactService, ContactService>();
        builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddAutoMapper(typeof(Mapper));
    }
}
