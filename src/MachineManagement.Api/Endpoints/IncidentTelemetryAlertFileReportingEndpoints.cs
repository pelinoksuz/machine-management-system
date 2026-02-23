using MachineManagement.Api.Contracts;
using MachineManagement.Api.Data;
using MachineManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineManagement.Api.Endpoints;

public static class IncidentTelemetryAlertFileReportingEndpoints
{
    public static RouteGroupBuilder MapIncidentEndpoints(this RouteGroupBuilder api)
    {
        var incidents = api.MapGroup("/incidents");
        incidents.MapGet("", async (AppDbContext db) => Results.Ok(await db.Incidents.ToListAsync()));
        incidents.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var incident = await db.Incidents.FindAsync(id);
            return incident is null ? Results.NotFound() : Results.Ok(incident);
        });
        incidents.MapPost("", async (IncidentRequest req, AppDbContext db) =>
        {
            var incident = new Incident { MachineId = req.MachineId, Summary = req.Summary, Severity = req.Severity };
            db.Incidents.Add(incident);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/incidents/{incident.Id}", incident);
        });
        incidents.MapPut("/{id:guid}", async (Guid id, IncidentRequest req, AppDbContext db) =>
        {
            var incident = await db.Incidents.FindAsync(id);
            if (incident is null) return Results.NotFound();
            incident.MachineId = req.MachineId;
            incident.Summary = req.Summary;
            incident.Severity = req.Severity;
            await db.SaveChangesAsync();
            return Results.Ok(incident);
        });
        incidents.MapPatch("/{id:guid}/status", async (Guid id, StatusRequest req, AppDbContext db) =>
        {
            var incident = await db.Incidents.FindAsync(id);
            if (incident is null) return Results.NotFound();
            incident.Status = req.Status;
            await db.SaveChangesAsync();
            return Results.Ok(incident);
        });

        return api;
    }

    public static RouteGroupBuilder MapTelemetryEndpoints(this RouteGroupBuilder api)
    {
        api.MapPost("/telemetry", async (TelemetryRequest req, AppDbContext db) =>
        {
            var telemetry = new TelemetryReading
            {
                MachineId = req.MachineId,
                TimestampUtc = req.TimestampUtc,
                Key = req.Key,
                Value = req.Value
            };
            db.TelemetryReadings.Add(telemetry);
            await db.SaveChangesAsync();
            return Results.Accepted($"/api/v1/machines/{req.MachineId}/telemetry", telemetry);
        });

        return api;
    }

    public static RouteGroupBuilder MapAlertEndpoints(this RouteGroupBuilder api)
    {
        var alerts = api.MapGroup("/alerts");
        alerts.MapGet("", async (AppDbContext db) => Results.Ok(await db.Alerts.ToListAsync()));
        alerts.MapPost("", async (AlertRequest req, AppDbContext db) =>
        {
            var alert = new Alert { MachineId = req.MachineId, Message = req.Message, Severity = req.Severity };
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/alerts/{alert.Id}", alert);
        });
        alerts.MapPatch("/{id:guid}/acknowledge", async (Guid id, AppDbContext db) =>
        {
            var alert = await db.Alerts.FindAsync(id);
            if (alert is null) return Results.NotFound();
            alert.Status = "Acknowledged";
            await db.SaveChangesAsync();
            return Results.Ok(alert);
        });
        alerts.MapPatch("/{id:guid}/resolve", async (Guid id, AppDbContext db) =>
        {
            var alert = await db.Alerts.FindAsync(id);
            if (alert is null) return Results.NotFound();
            alert.Status = "Resolved";
            await db.SaveChangesAsync();
            return Results.Ok(alert);
        });

        return api;
    }

    public static RouteGroupBuilder MapFileEndpoints(this RouteGroupBuilder api)
    {
        var files = api.MapGroup("/files");
        files.MapPost("", async (FileRequest req, AppDbContext db) =>
        {
            var file = new FileDocument { MachineId = req.MachineId, FileName = req.FileName, ContentType = req.ContentType };
            db.Files.Add(file);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/files/{file.Id}", file);
        });
        files.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var file = await db.Files.FindAsync(id);
            return file is null ? Results.NotFound() : Results.Ok(file);
        });
        files.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var file = await db.Files.FindAsync(id);
            if (file is null) return Results.NotFound();
            db.Files.Remove(file);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
        return api;
    }

    public static RouteGroupBuilder MapReportingEndpoints(this RouteGroupBuilder api)
    {
        api.MapGet("/audit-logs", () => Results.Ok(Array.Empty<object>()));

        var dashboard = api.MapGroup("/dashboard");
        dashboard.MapGet("/overview", async (AppDbContext db) => Results.Ok(new
        {
            machines = await db.Machines.CountAsync(),
            openIncidents = await db.Incidents.CountAsync(x => x.Status == "Open"),
            openWorkOrders = await db.WorkOrders.CountAsync(x => x.Status == "Open")
        }));

        var reports = api.MapGroup("/reports");
        reports.MapGet("/availability", async (AppDbContext db) => Results.Ok(new { machineCount = await db.Machines.CountAsync(), period = "last_30_days" }));
        reports.MapGet("/failures", async (AppDbContext db) => Results.Ok(new { failures = await db.Incidents.CountAsync(), period = "last_30_days" }));
        reports.MapGet("/maintenance-cost", async (AppDbContext db) => Results.Ok(new { totalWorkOrders = await db.WorkOrders.CountAsync(), period = "last_30_days" }));

        return api;
    }
}
