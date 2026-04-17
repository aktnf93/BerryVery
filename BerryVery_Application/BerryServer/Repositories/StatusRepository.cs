using BerryServer.Connections;

namespace BerryServer.Repositories
{
    public class StatusRepository : RepositoryBaseEx<StatusRepository>
    {
        public StatusRepository(ILogger<StatusRepository> logger, DatabaseConnection db) : base(logger, db)
        {
        }


    }
}
