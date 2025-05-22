using UserContacts.Bll.Dtos;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.RoleRepositories;

public interface IRoleService
{
    Task<long> AddRole(RoleCreateDto role);
    Task<List<UserGetDto>> GetAllUsersByRoleAsync(string role);
    Task<List<UserRole>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
    Task DeleteRole(string role);
}
