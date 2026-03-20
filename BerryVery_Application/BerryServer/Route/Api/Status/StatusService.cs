using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.Status
{
    public class StatusService : ServiceBaseEx<StatusService, StatusRepository>
    {
        public StatusService(ILogger<StatusService> logger, StatusRepository repository, SocketCommService socket) : base(logger, repository, socket)
        {
        }
    }
}
