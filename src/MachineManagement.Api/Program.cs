using MachineManagement.Api.Data;
using MachineManagement.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MachineManagementDb")));

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.MapApiEndpoints();

app.Run();
