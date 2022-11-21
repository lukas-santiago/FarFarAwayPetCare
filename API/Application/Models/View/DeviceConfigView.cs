namespace Application.Models.View
{
    public class DeviceConfigView
    {
        public int DeviceId { get; set; }
        public List<DeviceConfigSimplified> deviceConfigs { get; set; }
    }
    public class DeviceConfigSimplified
    {
        public int Periodicidade { get; set; }
        public string extras { get; set; } = "";
        public int DeviceConfigTypeId { get; set; }
    }
}
