using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.User
{
    public class UserRepository : RepositoryBaseEx<UserRepository>
    {
        public UserRepository(ILogger<UserRepository> logger, DatabaseCommService db) : base(logger, db)
        {
        }
    }
}
