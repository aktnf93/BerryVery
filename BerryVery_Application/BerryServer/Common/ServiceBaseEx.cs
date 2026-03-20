using BerryServer.CommServices;

namespace BerryServer.Common
{
    public class ServiceBaseEx<Service, Repository>
    {
        protected readonly ILogger<Service> _logger;
        protected readonly Repository _repository;
        protected readonly SocketCommService _socket;

        public ServiceBaseEx(ILogger<Service> logger, Repository repository, SocketCommService socket)
        {
            this._logger = logger;
            this._repository = repository;
            this._socket = _socket;
        }
    }
}
