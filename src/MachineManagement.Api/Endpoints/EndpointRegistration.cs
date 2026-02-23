namespace MachineManagement.Api.Endpoints;

public static class EndpointRegistration
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1");
        api.MapSystemEndpoints();
        api.MapAuthEndpoints();
        api.MapUsersEndpoints();
        api.MapRolesEndpoints();
        api.MapMachineEndpoints();
        api.MapMaintenanceEndpoints();
        api.MapIncidentEndpoints();
        api.MapTelemetryEndpoints();
        api.MapAlertEndpoints();
        api.MapFileEndpoints();
        api.MapReportingEndpoints();
    }
}
