using BerryServer.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BerryServer.Route.Api.Device
{


    /// <summary>
    /// - api/device/port <br/>
    /// - api/device/gate <br/>
    /// - api/device/ctrl <br/>
    /// - api/device/sub
    /// </summary>
    [Route("api/[controller]")]
    //[ApiController]
    public class DeviceController : ControllerBaseEx<DeviceController, DeviceService>
    {
        public DeviceController(ILogger<DeviceController> logger, DeviceService service) : base(logger, service)
        {
        }


        public class Model
        {
            public string Name { get; set; } = string.Empty;
        }


        [HttpGet("view")]
        public IActionResult GetDevice([FromBody] object data)
        {
            Console.WriteLine($"HTTP > FromBody={data}");
            return base.Ok();
        }












        [HttpGet("port")]
        public IActionResult GetPort()
        {
            var result = base._service.GetDevicePort("1");

            return base.Ok(result);
        }

        [HttpPost("port")]
        public IActionResult PostPort([FromBody] string value)
        {
            return base.Ok(1);
        }

        [HttpPut("port/{id}")]
        public IActionResult PutPort(int id, [FromBody] string value)
        {
            return base.Ok(1);
        }

        [HttpDelete("port/{id}")]
        public IActionResult DeletePort(int id)
        {
            return base.Ok(1);
        }


        [HttpGet("gate")]
        public IActionResult GetGate()
        {
            return base.Ok(1);
        }
    }
}
