using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserContacts.Bll.Services.RoleRepositories;
using UserContacts.Bll.Services.UserServices;

namespace UserContacts.Server.Endpoints;

public static class AdminEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/user")
            .RequireAuthorization()
            .WithTags("User Management");

        userGroup.MapDelete("/delete", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long userId, HttpContext httpContext, IUserService userService) =>
        {
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            await userService.DeleteUserByIdAsync(userId);
            return Results.Ok();
        })
        .WithName("DeleteUser");

        userGroup.MapPatch("/updateRole", [Authorize(Roles = "SuperAdmin")]
        async (long userId, string userRole, IUserService userService) =>
        {
            await userService.UpdateUserRoleAsync(userId, userRole);
            return Results.Ok();
        })
        .WithName("UpdateUserRole");

        userGroup.MapGet("/get-all-users-by-role", [Authorize(Roles = "Admin, SuperAdmin")][ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async (string role, IRoleService _roleService) =>
        {
            var users = await _roleService.GetAllUsersByRoleAsync(role);
            return Results.Ok(users);
        })
        .WithName("GetUsersByRole");
    }
}
