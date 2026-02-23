namespace MachineManagement.Api.Endpoints;

public static class SystemEndpoints
{
    public static RouteGroupBuilder MapSystemEndpoints(this RouteGroupBuilder api)
    {
        api.MapGet("/health/live", () => Results.Ok(new { status = "live", at = DateTimeOffset.UtcNow }));
        api.MapGet("/health/ready", () => Results.Ok(new { status = "ready", at = DateTimeOffset.UtcNow }));
        api.MapGet("/version", () => Results.Ok(new { name = "MachineManagement.Api", version = "1.0.0" }));
        return api;
    }
}
