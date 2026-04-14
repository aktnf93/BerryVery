using BerryServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers.Api
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
