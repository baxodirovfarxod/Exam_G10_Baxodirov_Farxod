using UserContacts.Bll.Dtos;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.UserServices;

public interface IUserService
{
    Task<long> AddUserAync(UserCreateDto user);
    Task<UserGetDto> GetUserByIdAync(long id);
    Task<User> GetUserByUsernameAync(string userName);
    Task UpdateUserRoleAsync(long userId, string userRole);
    Task DeleteUserByIdAsync(long userId);
    Task<bool> CheckUserById(long userId);
    Task<bool> CheckUsernameExists(string username);
    Task<bool> CheckEmailExists(string email);
}
