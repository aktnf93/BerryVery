using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.Status
{
    public class StatusRepository : RepositoryBaseEx<StatusRepository>
    {
        public StatusRepository(ILogger<StatusRepository> logger, DatabaseCommService db) : base(logger, db)
        {
        }


    }
}
