using MachineManagement.Api.Contracts;
using MachineManagement.Api.Data;
using MachineManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineManagement.Api.Endpoints;

public static class MaintenanceEndpoints
{
    public static RouteGroupBuilder MapMaintenanceEndpoints(this RouteGroupBuilder api)
    {
        var maintenancePlans = api.MapGroup("/maintenance-plans");
        maintenancePlans.MapGet("", async (AppDbContext db) => Results.Ok(await db.MaintenancePlans.ToListAsync()));
        maintenancePlans.MapPost("", async (MaintenancePlanRequest req, AppDbContext db) =>
        {
            var plan = new MaintenancePlan { Name = req.Name, IntervalDays = req.IntervalDays };
            db.MaintenancePlans.Add(plan);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/maintenance-plans/{plan.Id}", plan);
        });
        maintenancePlans.MapPut("/{id:guid}", async (Guid id, MaintenancePlanRequest req, AppDbContext db) =>
        {
            var plan = await db.MaintenancePlans.FindAsync(id);
            if (plan is null) return Results.NotFound();
            plan.Name = req.Name;
            plan.IntervalDays = req.IntervalDays;
            await db.SaveChangesAsync();
            return Results.Ok(plan);
        });

        var workOrders = api.MapGroup("/work-orders");
        workOrders.MapGet("", async (AppDbContext db) => Results.Ok(await db.WorkOrders.ToListAsync()));
        workOrders.MapPost("", async (WorkOrderRequest req, AppDbContext db) =>
        {
            var workOrder = new WorkOrder { MachineId = req.MachineId, Title = req.Title, Priority = req.Priority };
            db.WorkOrders.Add(workOrder);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/work-orders/{workOrder.Id}", workOrder);
        });
        workOrders.MapPut("/{id:guid}", async (Guid id, WorkOrderRequest req, AppDbContext db) =>
        {
            var workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder is null) return Results.NotFound();
            workOrder.MachineId = req.MachineId;
            workOrder.Title = req.Title;
            workOrder.Priority = req.Priority;
            await db.SaveChangesAsync();
            return Results.Ok(workOrder);
        });
        workOrders.MapPatch("/{id:guid}/status", async (Guid id, StatusRequest req, AppDbContext db) =>
        {
            var workOrder = await db.WorkOrders.FindAsync(id);
            if (workOrder is null) return Results.NotFound();
            workOrder.Status = req.Status;
            await db.SaveChangesAsync();
            return Results.Ok(workOrder);
        });

        return api;
    }
}
