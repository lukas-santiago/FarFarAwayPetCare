using Microsoft.Extensions.Hosting;

namespace Application.Models;

public class Device : BaseModel
{
    public string Nome { get; set; }
    public string UniqueDeviceId { get; set; }
    public bool Ativo { get; set; } = true;

    public List<DeviceConfig>? DeviceConfig { get; set; }
}
