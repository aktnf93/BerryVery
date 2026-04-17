using BerryServer.Connections;

namespace BerryServer.Repositories
{
    public class ZoneRepository : RepositoryBaseEx<ZoneRepository>
    {
        public ZoneRepository(ILogger<ZoneRepository> logger, DatabaseConnection db) : base(logger, db)
        {
        }
    }
}
