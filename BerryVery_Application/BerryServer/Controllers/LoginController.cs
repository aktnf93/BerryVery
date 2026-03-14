using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var scheme = HttpContext.Request.Scheme;
            Console.WriteLine("Forwarded Headers > {0}, {1}", ip, scheme);

            return base.Ok();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
