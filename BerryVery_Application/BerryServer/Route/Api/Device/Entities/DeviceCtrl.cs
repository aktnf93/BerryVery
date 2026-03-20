namespace BerryServer.Route.Api.Device.Entities
{
    public class DeviceCtrl
    {
        public int Id { get; set; }
        public int GateId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Address { get; set; }
        public int Status { get; set; }
    }
}
