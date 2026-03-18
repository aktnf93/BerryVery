using BerryServer.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BerryServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly DeviceService _service;

        public DeviceController(ILogger<DeviceController> logger, DeviceService service)
        {
            this._logger = logger;
            this._service = service;
        }

        // GET: api/<DeviceController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = this._service.GetDevicePort("1");

            return base.Ok(result);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return base.Ok(1);
        }

        // PUT api/<DeviceController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return base.Ok(1);
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return base.Ok(1);
        }
    }
}
