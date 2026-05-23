using BerryServer.Application.Repositories;
using BerryServer.Application.Services;
using BerryServer.Domain.Entities;
using BerryServer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BerryServer.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly RoomService _service;

        public RoomController(ILogger<RoomController> logger, RoomService service)
        {
            this._logger = logger;
            this._service = service;
        }

        [HttpGet]
        public IAsyncEnumerable<Room> Get(CancellationToken cancellationToken)
        {
            return this._service.GetRooms(cancellationToken);
        }

        [HttpPost]
        public IActionResult Post([FromBody] object[] value)
        {
            Console.WriteLine("POST !");
            var queryString = string.Join("&", value);

            Console.WriteLine(queryString);

            return base.Ok(1);
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return base.Ok(1);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return base.Ok(1);
        }
    }
}
