using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.Zone
{
    public class ZoneService : ServiceBaseEx<ZoneService, ZoneRepository>
    {
        public ZoneService(ILogger<ZoneService> logger, ZoneRepository repository, SocketCommService socket) : base(logger, repository, socket)
        {
        }
    }
}
