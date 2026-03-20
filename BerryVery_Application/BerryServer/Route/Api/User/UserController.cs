using BerryServer.Common;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route.Api.User
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBaseEx<UserController, UserService>
    {
        public UserController(ILogger<UserController> logger, UserService service) : base(logger, service)
        {
        }
    }
}
