using Microsoft.Extensions.Hosting;

namespace Application.Models;

public class BaseModel
{
    public int Id { get; set; }
    public string? User { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedOn { get; set; }
}
public class Device : BaseModel
{
    public string? Nome { get; set; }

    public List<DeviceConfig>? DeviceConfig { get; set; }
}
public class DeviceConfigType : BaseModel
{
    public string? Nome { get; set; }

}
public class DeviceConfig : BaseModel
{
    public int Periodicidade { get; set; }

    public int DeviceId { get; set; }
    public Device? Device { get; set; }

    public int DeviceConfigTypeId { get; set; }
    public DeviceConfigType? DeviceConfigType { get; set; }
    
    public List<DeviceData>? DeviceData { get; set; }

}

public class DeviceData : BaseModel
{
    public string? Nome { get; set; }
    public int Value { get; set; }

    public int DeviceConfigId { get; set; }
    public DeviceConfig? DeviceConfig { get; set; }
}

public class Image : BaseModel
{
    public string? Nome { get; set; }
    public string? Base64 { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Role { get; set; }
}
