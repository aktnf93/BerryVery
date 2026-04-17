using BerryServer.Connections;
using BerryServer.Repositories;

namespace BerryServer.Services
{
    public class DeviceService : ServiceBaseEx<DeviceService, DeviceRepository>
    {
        public DeviceService(ILogger<DeviceService> logger, DeviceRepository repository, TcpSocketConnection socket) : base(logger, repository, socket)
        {
        }

        public object GetDevicePort(string deviceId)
        {
            var ports = this._repository.GetDevicePort();

            return ports;
        }
    }
}
