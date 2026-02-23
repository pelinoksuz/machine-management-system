namespace MachineManagement.Api.Contracts;

public record LoginRequest(string UserName, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record UserCreateRequest(string UserName, string Email, IEnumerable<Guid>? RoleIds);
public record RoleRequest(string Name);
public record MachineCreateRequest(string Name, string Model, string SiteId, string Criticality);
public record MachineStatusRequest(string Status);
public record ComponentRequest(string Name, string SerialNumber);
public record MachineConfigurationRequest(string TimeZone, int SamplingIntervalSeconds);
public record MaintenancePlanRequest(string Name, int IntervalDays);
public record WorkOrderRequest(Guid MachineId, string Title, string Priority);
public record IncidentRequest(Guid MachineId, string Summary, string Severity);
public record TelemetryRequest(Guid MachineId, DateTimeOffset TimestampUtc, string Key, decimal Value);
public record AlertRequest(Guid MachineId, string Message, string Severity);
public record FileRequest(Guid MachineId, string FileName, string ContentType);
public record StatusRequest(string Status);
public record PagedResponse<T>(IEnumerable<T> Data, int Page, int PageSize, int TotalCount);
