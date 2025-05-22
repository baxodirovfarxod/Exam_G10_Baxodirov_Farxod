using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserContacts.Bll.Dtos;
using UserContacts.Bll.Services;
using UserContacts.Bll.Services.RoleRepositories;

namespace UserContacts.Server.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/role")
            .RequireAuthorization()
            .WithTags("UserRole Management");

        userGroup.MapGet("/get-all-roles", [Authorize(Roles = "Admin, SuperAdmin")]
        async (IRoleService _roleService) =>
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Results.Ok(roles);
        })
        .WithName("GetAllRoles");

        userGroup.MapGet("/post-role", [Authorize(Roles = "SuperAdmin")]
        async (RoleCreateDto role,IRoleService _roleService) =>
        {
            var roles = await _roleService.AddRole(role);
            return Results.Ok(roles);
        })
        .WithName("PostRole");

        userGroup.MapGet("/delete - role", [Authorize(Roles = "Admin, SuperAdmin")]
        async (string role, IRoleService _roleService) =>
        {
            await _roleService.DeleteRole(role);
            return Results.Ok();
        })
        .WithName("DeleteRoles");
    }
}
