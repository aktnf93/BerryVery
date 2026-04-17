namespace BerrDeviceSimulator.Models
{
    public class DeviceGate
    {
        public uint GateId { get; set; }
        public string GateName { get; set; } = string.Empty;
        public uint GateType { get; set; }
        public uint GateAddress { get; set; }
        public uint GateStatus { get; set; }
    }
}
