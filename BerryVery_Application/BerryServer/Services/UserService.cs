using BerryServer.Connections;
using BerryServer.Repositories;

namespace BerryServer.Services
{
    public class UserService : ServiceBaseEx<UserService, UserRepository>
    {
        public UserService(ILogger<UserService> logger, UserRepository repository, TcpSocketConnection socket) : base(logger, repository, socket)
        {
        }
    }
}
