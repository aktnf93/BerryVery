namespace BerrDeviceSimulator.Models
{
    public class DeviceSub
    {
        public uint SubId { get; set; }
        public string SubName { get; set; } = string.Empty;
        public uint SubType { get; set; }
        public uint SubAddress { get; set; }
        public uint SubStatus { get; set; }
    }
}
