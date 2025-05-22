using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContacts.Bll.Dtos;
using UserContacts.Core.Errors;
using UserContacts.Dal;
using UserContacts.Dal.Entities;

namespace UserContacts.Bll.Services.RoleRepositories;

public class RoleService : IRoleService
{
    private readonly ILogger<RoleService> logger;
    private readonly MainContext mainContext;
    private readonly IMapper mapper;

    public RoleService(MainContext mainContext, IMapper mapper)
    {
        this.mainContext = mainContext;
        this.mapper = mapper;
    }

    public async Task<long> AddRole(RoleCreateDto role)
    {
        var roleEntity = mapper.Map<UserRole>(role);
        mainContext.UserRoles.Add(roleEntity);
        await mainContext.SaveChangesAsync();
        return roleEntity.UserRoleId;
    }

    public async Task DeleteRole(string role)
    {
        var roleEntity = await mainContext.UserRoles.FirstOrDefaultAsync(r => r.RoleName == role);
        logger.LogInformation("Delete role with name: {RoleName}", roleEntity.RoleName);
        mainContext.UserRoles.Remove(roleEntity);
        await mainContext.SaveChangesAsync();
    }

    public async Task<List<UserRole>> GetAllRolesAsync()
    {
        return await mainContext.UserRoles
            .ToListAsync();
    }

    public async Task<List<UserGetDto>> GetAllUsersByRoleAsync(string role)
    {
        var roleId = await GetRoleIdAsync(role);

        var users = await mainContext.Users.Where(u => u.UserRoleId == roleId).ToListAsync();

        return mapper.Map<List<UserGetDto>>(users);
    }

    public async Task<long> GetRoleIdAsync(string role)
    {
        var roleById = await mainContext.UserRoles.FirstOrDefaultAsync(r => r.RoleName == role);
        if (roleById == null)
        {
            throw new EntityNotFoundException();
        }

        return roleById.UserRoleId;
    }
}
