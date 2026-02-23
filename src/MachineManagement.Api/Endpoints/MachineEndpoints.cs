using MachineManagement.Api.Contracts;
using MachineManagement.Api.Data;
using MachineManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineManagement.Api.Endpoints;

public static class MachineEndpoints
{
    public static RouteGroupBuilder MapMachineEndpoints(this RouteGroupBuilder api)
    {
        var machines = api.MapGroup("/machines");

        machines.MapGet("", async (AppDbContext db, string? status, string? model, int page = 1, int pageSize = 20) =>
        {
            var query = db.Machines.AsQueryable();
            if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
            if (!string.IsNullOrWhiteSpace(model)) query = query.Where(x => x.Model.Contains(model));

            var total = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Results.Ok(new PagedResponse<Machine>(data, page, pageSize, total));
        });

        machines.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var machine = await db.Machines.Include(x => x.Components).Include(x => x.Configuration).FirstOrDefaultAsync(x => x.Id == id);
            return machine is null ? Results.NotFound() : Results.Ok(machine);
        });

        machines.MapPost("", async (MachineCreateRequest req, AppDbContext db) =>
        {
            var machine = new Machine { Name = req.Name, Model = req.Model, SiteId = req.SiteId, Criticality = req.Criticality };
            db.Machines.Add(machine);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/machines/{machine.Id}", machine);
        });

        machines.MapPut("/{id:guid}", async (Guid id, MachineCreateRequest req, AppDbContext db) =>
        {
            var machine = await db.Machines.FindAsync(id);
            if (machine is null) return Results.NotFound();
            machine.Name = req.Name;
            machine.Model = req.Model;
            machine.SiteId = req.SiteId;
            machine.Criticality = req.Criticality;
            await db.SaveChangesAsync();
            return Results.Ok(machine);
        });

        machines.MapPatch("/{id:guid}/status", async (Guid id, MachineStatusRequest req, AppDbContext db) =>
        {
            var machine = await db.Machines.FindAsync(id);
            if (machine is null) return Results.NotFound();
            machine.Status = req.Status;
            await db.SaveChangesAsync();
            return Results.Ok(machine);
        });

        machines.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var machine = await db.Machines.FindAsync(id);
            if (machine is null) return Results.NotFound();
            db.Machines.Remove(machine);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        machines.MapGet("/{id:guid}/components", async (Guid id, AppDbContext db) =>
            Results.Ok(await db.Components.Where(x => x.MachineId == id).ToListAsync()));

        machines.MapPost("/{id:guid}/components", async (Guid id, ComponentRequest req, AppDbContext db) =>
        {
            if (!await db.Machines.AnyAsync(x => x.Id == id)) return Results.NotFound();
            var component = new MachineComponent { MachineId = id, Name = req.Name, SerialNumber = req.SerialNumber };
            db.Components.Add(component);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/machines/{id}/components/{component.Id}", component);
        });

        machines.MapPut("/{id:guid}/components/{componentId:guid}", async (Guid id, Guid componentId, ComponentRequest req, AppDbContext db) =>
        {
            var component = await db.Components.FirstOrDefaultAsync(x => x.Id == componentId && x.MachineId == id);
            if (component is null) return Results.NotFound();
            component.Name = req.Name;
            component.SerialNumber = req.SerialNumber;
            await db.SaveChangesAsync();
            return Results.Ok(component);
        });

        machines.MapDelete("/{id:guid}/components/{componentId:guid}", async (Guid id, Guid componentId, AppDbContext db) =>
        {
            var component = await db.Components.FirstOrDefaultAsync(x => x.Id == componentId && x.MachineId == id);
            if (component is null) return Results.NotFound();
            db.Components.Remove(component);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        machines.MapGet("/{id:guid}/configuration", async (Guid id, AppDbContext db) =>
        {
            var config = await db.Configurations.FirstOrDefaultAsync(x => x.MachineId == id);
            return config is null ? Results.NotFound() : Results.Ok(config);
        });

        machines.MapPut("/{id:guid}/configuration", async (Guid id, MachineConfigurationRequest req, AppDbContext db) =>
        {
            var config = await db.Configurations.FirstOrDefaultAsync(x => x.MachineId == id);
            if (config is null)
            {
                config = new MachineConfiguration { MachineId = id };
                db.Configurations.Add(config);
            }

            config.TimeZone = req.TimeZone;
            config.SamplingIntervalSeconds = req.SamplingIntervalSeconds;
            await db.SaveChangesAsync();
            return Results.Ok(config);
        });

        machines.MapGet("/{id:guid}/telemetry", async (Guid id, AppDbContext db) =>
            Results.Ok(await db.TelemetryReadings.Where(x => x.MachineId == id).OrderByDescending(x => x.TimestampUtc).Take(100).ToListAsync()));

        machines.MapGet("/{id:guid}/metrics", async (Guid id, AppDbContext db) =>
        {
            var telemetryCount = await db.TelemetryReadings.CountAsync(x => x.MachineId == id);
            var openIncidents = await db.Incidents.CountAsync(x => x.MachineId == id && x.Status == "Open");
            return Results.Ok(new { machineId = id, telemetryCount, openIncidents });
        });

        machines.MapGet("/{id:guid}/files", async (Guid id, AppDbContext db) =>
            Results.Ok(await db.Files.Where(x => x.MachineId == id).ToListAsync()));

        machines.MapGet("/{id:guid}/history", async (Guid id, AppDbContext db) =>
            Results.Ok(await db.TelemetryReadings.Where(x => x.MachineId == id).OrderByDescending(x => x.TimestampUtc).Take(20).ToListAsync()));

        return api;
    }
}
