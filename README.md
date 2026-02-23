# Machine Management System API

.NET 8 Minimal API + MSSQL (EF Core) starter project.

## Folder Structure

- `src/MachineManagement.Api/Program.cs`: Bootstrap and DI only
- `src/MachineManagement.Api/Endpoints/*`: Endpoint groups
- `src/MachineManagement.Api/Data/*`: EF Core `DbContext`

- `src/MachineManagement.Api/Domain/*`: Entity classes

- `src/MachineManagement.Api/Contracts/*`: Request/response contracts

## MSSQL Connection

Connection string in `src/MachineManagement.Api/appsettings.json`:

- `ConnectionStrings:MachineManagementDb`

## Execution

```bash
dotnet restore` src/MachineManagement.Api/MachineManagement.Api.csproj
dotnet ef migrations add InitialCreate --project src/MachineManagement.Api
dotnet ef database update --project src/MachineManagement.Api
dotnet run --project src/MachineManagement.Api/MachineManagement.Api.csproj
```

Swagger: `/swagger`
