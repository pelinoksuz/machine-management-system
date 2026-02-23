namespace MachineManagement.Api.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Role> Roles { get; set; } = [];
}

public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}

public class Machine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SiteId { get; set; } = string.Empty;
    public string Status { get; set; } = "Running";
    public string Criticality { get; set; } = "Medium";
    public List<MachineComponent> Components { get; set; } = [];
    public MachineConfiguration? Configuration { get; set; }
}

public class MachineComponent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public Machine? Machine { get; set; }
}

public class MachineConfiguration
{
    public Guid MachineId { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public int SamplingIntervalSeconds { get; set; } = 5;
    public Machine? Machine { get; set; }
}

public class MaintenancePlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int IntervalDays { get; set; }
}

public class WorkOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Priority { get; set; } = "Normal";
    public Machine? Machine { get; set; }
}

public class Incident
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Severity { get; set; } = "Medium";
    public Machine? Machine { get; set; }
}

public class TelemetryReading
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public DateTimeOffset TimestampUtc { get; set; }
    public string Key { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public Machine? Machine { get; set; }
}

public class Alert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Severity { get; set; } = "Medium";
    public Machine? Machine { get; set; }
}

public class FileDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MachineId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public Machine? Machine { get; set; }
}
