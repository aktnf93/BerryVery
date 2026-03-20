namespace BerryServer.Route.Api.Device.Entities
{
    public class DeviceGate
    {
        public int Id { get; set; }
        public int PortId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Address { get; set; }
        public int Status { get; set; }
    }
}
