namespace BerrDeviceSimulator.Models
{
    public class DevicePort
    {
        public uint PortId { get; set; }
        public string PortName { get; set; } = string.Empty;
        public uint PortType { get; set; }
        public string PortAddress { get; set; } = string.Empty;
        public uint PortStatus { get; set; }
    }
}
