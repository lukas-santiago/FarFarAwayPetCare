using System.Dynamic;

namespace SimuladorDoFirmware.Models;
public class Device : BaseModel
{
    public string Nome { get; set; }
    public string UniqueDeviceId { get; set; }
    public bool Ativo { get; set; } = true;

    public List<DeviceConfig>? DeviceConfig { get; set; }

    public static explicit operator Device(ExpandoObject v)
    {
        throw new NotImplementedException();
    }
}
