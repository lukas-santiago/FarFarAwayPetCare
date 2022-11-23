namespace SimuladorDoFirmware.Models;

public class DeviceData : BaseModel
{
    public string? Nome { get; set; }
    public double Value { get; set; }

    public int DeviceConfigId { get; set; }
}
