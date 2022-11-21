namespace Application.Models;

public class DeviceConfig : BaseModel
{
    public int Periodicidade { get; set; }
    public string extras { get; set; } = "";

    public int DeviceId { get; set; }
    public Device? Device { get; set; }

    public int DeviceConfigTypeId { get; set; }
    public DeviceConfigType? DeviceConfigType { get; set; }
    
    public List<DeviceData>? DeviceData { get; set; }

}
