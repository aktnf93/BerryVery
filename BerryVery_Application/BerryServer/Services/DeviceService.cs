using BerryServer.CommServices;
using BerryServer.Repositories;

namespace BerryServer.Services
{
    public class DeviceService
    {
        private readonly ILogger<DeviceService> _logger;
        private readonly DeviceRepository _repository;
        private readonly SocketCommService _comm;

        public DeviceService(ILogger<DeviceService> logger, 
            DeviceRepository repository, SocketCommService comm)
        {
            this._logger = logger;
            this._repository = repository;
            this._comm = comm;
        }

        public object GetDevicePort(string deviceId)
        {
            var ports = this._repository.GetDevicePort();

            return ports;
        }
    }
}
