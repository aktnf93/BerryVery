using BerryServer.Connections;
using BerryServer.Repositories;

namespace BerryServer.Services
{
    public class ZoneService : ServiceBaseEx<ZoneService, ZoneRepository>
    {
        public ZoneService(ILogger<ZoneService> logger, ZoneRepository repository, TcpSocketConnection socket) : base(logger, repository, socket)
        {
        }
    }
}
