using BerryServer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
    public class ApiController : ControllerBase
    {
        [HttpPost("/api/user")]
        // [FromBody]를 붙이면 JSON 바디를 UserRequest 객체로 자동 변환해줍니다.
        public IActionResult GetUser([FromBody] UserRequest request)
        {
            Console.WriteLine($"Received name: {request.Name}");

            var msg = new MessageResponse() { Message = $"{request.Name}님, 환영합니다." };
            return base.Ok(msg);
        }
    }
}
