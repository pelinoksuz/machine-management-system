using MachineManagement.Api.Contracts;
using MachineManagement.Api.Data;
using MachineManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineManagement.Api.Endpoints;

public static class UserRoleEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder api)
    {
        var users = api.MapGroup("/users");

        users.MapGet("", async (AppDbContext db) => Results.Ok(await db.Users.Include(x => x.Roles).ToListAsync()));
        users.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var user = await db.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        });

        users.MapPost("", async (UserCreateRequest req, AppDbContext db) =>
        {
            var roles = req.RoleIds is null
                ? []
                : await db.Roles.Where(x => req.RoleIds.Contains(x.Id)).ToListAsync();

            var user = new User { UserName = req.UserName, Email = req.Email, Roles = roles };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/users/{user.Id}", user);
        });

        users.MapPut("/{id:guid}", async (Guid id, UserCreateRequest req, AppDbContext db) =>
        {
            var user = await db.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return Results.NotFound();
            user.UserName = req.UserName;
            user.Email = req.Email;
            user.Roles = req.RoleIds is null ? [] : await db.Roles.Where(x => req.RoleIds.Contains(x.Id)).ToListAsync();
            await db.SaveChangesAsync();
            return Results.Ok(user);
        });

        users.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) return Results.NotFound();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return api;
    }

    public static RouteGroupBuilder MapRolesEndpoints(this RouteGroupBuilder api)
    {
        var roles = api.MapGroup("/roles");

        roles.MapGet("", async (AppDbContext db) => Results.Ok(await db.Roles.ToListAsync()));
        roles.MapPost("", async (RoleRequest req, AppDbContext db) =>
        {
            var role = new Role { Name = req.Name };
            db.Roles.Add(role);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/roles/{role.Id}", role);
        });

        roles.MapPut("/{id:guid}", async (Guid id, RoleRequest req, AppDbContext db) =>
        {
            var role = await db.Roles.FindAsync(id);
            if (role is null) return Results.NotFound();
            role.Name = req.Name;
            await db.SaveChangesAsync();
            return Results.Ok(role);
        });

        roles.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var role = await db.Roles.FindAsync(id);
            if (role is null) return Results.NotFound();
            db.Roles.Remove(role);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return api;
    }
}
