# Machine Management System API

.NET 8 Minimal API + MSSQL (EF Core) başlangıç projesi.

## Klasör Yapısı

- `src/MachineManagement.Api/Program.cs`: sadece bootstrap ve DI
- `src/MachineManagement.Api/Endpoints/*`: endpoint grupları
- `src/MachineManagement.Api/Data/*`: EF Core `DbContext`
- `src/MachineManagement.Api/Domain/*`: entity sınıfları
- `src/MachineManagement.Api/Contracts/*`: request/response sözleşmeleri

## MSSQL bağlantısı

Connection string `src/MachineManagement.Api/appsettings.json` içinde:

- `ConnectionStrings:MachineManagementDb`

## Çalıştırma

```bash
dotnet restore src/MachineManagement.Api/MachineManagement.Api.csproj
dotnet ef migrations add InitialCreate --project src/MachineManagement.Api
dotnet ef database update --project src/MachineManagement.Api
dotnet run --project src/MachineManagement.Api/MachineManagement.Api.csproj
```

Swagger: `/swagger`
