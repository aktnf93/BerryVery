using BerryServer.Connections;

namespace BerryServer.Services
{
    public class ServiceBaseEx<Service, Repository>
    {
        protected readonly ILogger<Service> _logger;
        protected readonly Repository _repository;
        protected readonly TcpSocketConnection _socket;

        public ServiceBaseEx(ILogger<Service> logger, Repository repository, TcpSocketConnection socket)
        {
            this._logger = logger;
            this._repository = repository;
            this._socket = socket;
        }
    }
}
