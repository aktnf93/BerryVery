namespace BerrDeviceSimulator.Models
{
    public class DeviceCtrl
    {
        public uint CtrlId { get; set; }
        public string CtrlName { get; set; } = string.Empty;
        public uint CtrlType { get; set; }
        public uint CtrlAddress { get; set; }
        public uint CtrlStatus { get; set; }
    }
}
