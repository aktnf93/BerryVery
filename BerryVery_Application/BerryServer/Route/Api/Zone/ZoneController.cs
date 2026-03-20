using BerryServer.Common;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route.Api.Zone
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ZoneController : ControllerBaseEx<ZoneController, ZoneService>
    {
        public ZoneController(ILogger<ZoneController> logger, ZoneService service) : base(logger, service)
        {
        }
    }
}
