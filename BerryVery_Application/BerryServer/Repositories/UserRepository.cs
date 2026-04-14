using BerryServer.Connections;

namespace BerryServer.Repositories
{
    public class UserRepository : RepositoryBaseEx<UserRepository>
    {
        public UserRepository(ILogger<UserRepository> logger, DatabaseConnection db) : base(logger, db)
        {
        }
    }
}
