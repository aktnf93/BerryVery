using BerryServer.Common;
using BerryServer.Route.Api.Device;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route.Api.Status
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBaseEx<StatusController, DeviceService>
    {
        public StatusController(ILogger<StatusController> logger, DeviceService service) : base(logger, service)
        {
        }

        [HttpGet("status")]
        public IActionResult GetPort()
        {
            var result = base._service.GetDevicePort("1");

            return base.Ok(result);
        }

        [HttpPost("status")]
        public IActionResult PostPort([FromBody] string value)
        {
            return base.Ok(1);
        }

        [HttpPut("status/{id}")]
        public IActionResult PutPort(int id, [FromBody] string value)
        {
            return base.Ok(1);
        }

        [HttpDelete("status/{id}")]
        public IActionResult DeletePort(int id)
        {
            return base.Ok(1);
        }
    }
}
