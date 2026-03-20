using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.Device
{
    public class DeviceService : ServiceBaseEx<DeviceService, DeviceRepository>
    {
        public DeviceService(ILogger<DeviceService> logger, DeviceRepository repository, SocketCommService socket) : base(logger, repository, socket)
        {
        }

        public object GetDevicePort(string deviceId)
        {
            var ports = this._repository.GetDevicePort();

            return ports;
        }
    }
}
