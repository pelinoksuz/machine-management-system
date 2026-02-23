using MachineManagement.Api.Contracts;

namespace MachineManagement.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder api)
    {
        var auth = api.MapGroup("/auth");

        auth.MapPost("/login", (LoginRequest req) => Results.Ok(new
        {
            accessToken = $"demo-access-{req.UserName}",
            refreshToken = "demo-refresh-token",
            expiresAt = DateTimeOffset.UtcNow.AddHours(1)
        }));

        auth.MapPost("/refresh", (RefreshTokenRequest req) => Results.Ok(new
        {
            accessToken = "demo-access-token-new",
            refreshToken = req.RefreshToken,
            expiresAt = DateTimeOffset.UtcNow.AddHours(1)
        }));

        auth.MapPost("/logout", () => Results.NoContent());
        auth.MapGet("/me", () => Results.Ok(new { id = Guid.NewGuid(), userName = "admin", roles = new[] { "Admin" } }));

        return api;
    }
}
