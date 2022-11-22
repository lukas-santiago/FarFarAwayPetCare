namespace SimuladorDoFirmware.Models;

public class DeviceData : BaseModel
{
    public string? Nome { get; set; }
    public int Value { get; set; }

    public int DeviceConfigId { get; set; }
}
