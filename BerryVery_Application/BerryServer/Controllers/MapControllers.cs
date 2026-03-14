using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => base.Ok("ok");

        [HttpGet("{id}")]
        public IActionResult Get(int id) => base.Ok(id);

        [HttpGet("items/{id?}")]
        public IActionResult Get(int? id)
        {
            if (id.HasValue)
                return base.Ok(id.Value);
            else
                return base.Ok(new[] { "A", "B", "C" });
        }
    }

    [Route("services/[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet("select")]
        public IResult AllEventList() => Results.Ok("all event list");

        [HttpPost("setdata")]
        public IResult SetEventData() => Results.Ok("set event data");
    }

    [Route("staff")]
    public class Staff : ControllerBase
    {
        [HttpGet]
        public IResult StaffInfo() => Results.Ok("staff data");
    }

    [Route("manager")]
    public class Manager : Controller
    {
        [HttpGet]
        public IResult ManagerInfo() => Results.Ok("manager data");
    }

    [Route("user")]
    public class UserController
    {
        [HttpGet]
        public IResult UserInfo() => Results.Ok("user data");
    }

    public class SystemController
    {
        [HttpGet("system")]
        public IResult SystemInfo() => Results.Ok("system data");
    }

    [Route("table")]
    public class TableController
    {
        public IResult TableInfo() => Results.Ok("table data");
    }

    [Controller]
    [Route("monitoring")]
    public class MyData
    {
        [HttpGet("factory")]
        public IResult A() => Results.Ok("monitoring factory");

        [NonAction]
        public string GetInternalCode() => "SECRET-123";
    }

    [Controller]
    [ApiController]
    [Route("models")]
    public class Models
    {
        [HttpGet]
        public IResult GetModelInfo() => Results.Ok("model data");

        [HttpGet]
        public IResult GetModelData() => Results.Ok("model data");

        [HttpGet]
        public IResult GetModelLog() => Results.Ok("model data");

        [HttpGet]
        [ActionName("test")]
        public IResult GetModelCheck() => Results.Ok("model data");
    }

    [Route("myclass")]
    public class MyClassController : ControllerBase
    {
        [HttpGet("method")]
        public IResult Method() => Results.Ok();
    }

    [Route("api/[controller]")] // /api/storage
    public class StorageController : ControllerBase
    {
        // 1. 캐치올 사용: 하위 폴더 깊이에 상관없이 경로를 다 읽음
        [HttpGet("download/{*filePath}")]
        public IResult Download(string filePath) => Results.Ok($"Downloading: {filePath}");

        // 2. 제약 조건 사용: ID는 반드시 숫자여야 함
        [HttpGet("details/{id:int}")]
        public IResult GetDetails(int id) => Results.Ok($"ID is {id}");

        // 3. 토큰 활용: 메서드 이름이 자동으로 URL이 됨
        [HttpGet("[action]")] // /api/storage/checkstatus
        public IResult CheckStatus() => Results.Ok("Running");


        // 4. 슬래시(/)를 포함한 나머지 모든 경로
        [HttpGet("test/{*path}")] // /api/storage/test/ ...
        public IResult Test() => Results.Ok("Test");

        [HttpGet("[action]/{id=1}")] // 매개변수 기본값은 1
        public IResult Number(int id) => Results.Ok(id);
    }
}
