using BerryServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StatusController : ControllerBaseEx<StatusController, StatusService>
    {
        public StatusController(ILogger<StatusController> logger, StatusService service) : base(logger, service)
        {
        }

        [HttpGet("status")]
        public IActionResult GetPort()
        {
            // var result = base._service.GetDevicePort("1");

            return base.Ok(1);
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
