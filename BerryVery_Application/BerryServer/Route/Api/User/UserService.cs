using BerryServer.Common;
using BerryServer.CommServices;

namespace BerryServer.Route.Api.User
{
    public class UserService : ServiceBaseEx<UserService, UserRepository>
    {
        public UserService(ILogger<UserService> logger, UserRepository repository, SocketCommService socket) : base(logger, repository, socket)
        {
        }
    }
}
