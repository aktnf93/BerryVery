using BerryServer.Connections;
using BerryServer.Middleware;
using BerryServer.Repositories;
using BerryServer.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BerryServer
{
    public class TestObj
    {
        public TestObj()
        {
            Console.WriteLine("{0:HH:mm:ss} \t TestObj 생성자 호출", DateTime.Now);
        }
    }

    public class TestObjService
    {
        public TestObjService(TestObj testObj)
        {
            Console.WriteLine("{0:HH:mm:ss} \t TestObjService 생성자 호출", DateTime.Now);
        }
    }

    [Route("api/[Controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger, TestObj testObj, TestObjService testObjService)
        {
            Console.WriteLine("{0:HH:mm:ss} \t TestController 생성자 호출", DateTime.Now);
        }

        [HttpGet("name")]
        public IActionResult GetName()
        {
            return base.Ok("name");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // http://localhost:8016/api/test/name

            var builder = WebApplication.CreateBuilder(args);

            // 모든 한글(UnicodeRanges.All 또는 .Hangul)을 안전하게 인코딩하도록 설정
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));

            // _______________________________________________________________________________
            // DI 등록 : HTTP 요청 시 생성자를 통해 내려받을 타입 등록
            // AddSingleton : 단일 객체 내려받음
            // AddScoped : HTTP 요청 마다 새 객체 생성해서 내려받음
            builder.Services.AddSingleton<TcpSocketConnection>();
            builder.Services.AddSingleton<DatabaseConnection>();
            builder.Services.AddScoped<DeviceRepository>();

            builder.Services.AddSingleton<TestObj>();
            builder.Services.AddScoped<TestObjService>();
            //builder.Services.AddScoped<DeviceRepository>(sp =>
            //{
            //    return new DeviceRepository(
            //        sp.GetRequiredService<ILogger<DeviceRepository>>(),
            //        sp.GetRequiredService<DatabaseCommService>()
            //    );
            //});
            builder.Services.AddScoped<DeviceService>();

            // builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            // _______________________________________________________________________________
            // ASP.NET 백그라운드 작업
            builder.Services.AddHostedService<TcpSocketConnection>();


            builder.WebHost.ConfigureKestrel(o =>
            {
                o.ListenAnyIP(8016);
            });

            var app = builder.Build();

            // 공통 실행 (예외 처리용)
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // 공통 실행 (요청 검사용)
            app.UseMiddleware<RequestCheckMiddleware>();

            // 프로젝트 내 혹은 같은 실행 경로의 wwwroot 폴더에서 정적 파일을 제공
            // wwwroot 폴더에 있는 파일들은 URL 경로로 접근할 수 있다. (예: http://localhost:5000/index.html, http://localhost:5000/css/style.css)
            app.UseStaticFiles();

            // 어트리뷰트 라우팅을 사용하여 컨트롤러의 액션 메서드에 직접 URL 매핑
            app.MapControllers();





            app.MapGet("/token", (CancellationToken token, ClaimsPrincipal user) =>
            {
                var IsCancellationRequested = token.IsCancellationRequested; // 요청이 취소되었는지 여부를 확인하는 예시
                var CanBeCanceled = token.CanBeCanceled; // 토큰이 취소될 수 있는지 여부를 확인하는 예시
                // token.ThrowIfCancellationRequested(); // 요청이 취소된 경우 예외를 발생시키는 예시

                string Claims = JsonSerializer.Serialize(user);

                return new { IsCancellationRequested, CanBeCanceled, Claims };
            });

            // / 경로로 접근 시 예외를 발생시키는 테스트용 라우트
            app.Map("/", (HttpContext context) => throw new Exception("테스트 예외"));

            // /app 경로로 시작하는 모든 요청에 대해 wwwroot/index.html 파일을 반환
            app.MapFallbackToFile("/app/{*path}", "index.html");

            // /404 경로로 시작하는 모든 요청에 대해 404 Not Found 응답을 반환
            app.MapFallback("/404/{*path}", async (HttpContext context) => TypedResults.NotFound());

            // URL 매핑 실패 시 FallbackAction 메서드가 있는 FallbackController로 라우팅
            // app.MapFallbackToController("FallbackAction", "FallbackController");

            app.Run();
        }
    }
}
