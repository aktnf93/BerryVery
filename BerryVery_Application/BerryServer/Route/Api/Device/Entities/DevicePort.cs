namespace BerryServer.Route.Api.Device.Entities
{
    public class DevicePort
    {
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint Type { get; set; }
        public string Address { get; set; } = string.Empty;
        public uint Status { get; set; }
    }
}
