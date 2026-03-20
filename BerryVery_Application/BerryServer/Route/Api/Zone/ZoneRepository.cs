using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.Zone
{
    public class ZoneRepository : RepositoryBaseEx<ZoneRepository>
    {
        public ZoneRepository(ILogger<ZoneRepository> logger, DatabaseCommService db) : base(logger, db)
        {
        }
    }
}
