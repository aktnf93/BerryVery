using BerryServer.Connections;
using BerryServer.Repositories;

namespace BerryServer.Services
{
    public class StatusService : ServiceBaseEx<StatusService, StatusRepository>
    {
        public StatusService(ILogger<StatusService> logger, StatusRepository repository, TcpSocketConnection socket) : base(logger, repository, socket)
        {
        }
    }
}
