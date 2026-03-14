using BerryServer.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace BerryServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            // 모든 한글(UnicodeRanges.All 또는 .Hangul)을 안전하게 인코딩하도록 설정
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));

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

            // / 경로로 접근 시 예외를 발생시키는 테스트용 라우트
            app.Map("/", (HttpContext context) => throw new Exception("테스트 예외"));

            // /app 경로로 시작하는 모든 요청에 대해 wwwroot/index.html 파일을 반환
            app.MapFallbackToFile("/app/{*path}", "index.html");

            // /404 경로로 시작하는 모든 요청에 대해 404 Not Found 응답을 반환
            app.MapFallback("/404/{*path}", async (HttpContext context) => TypedResults.NotFound());

            // URL 매핑 실패 시 HandleFallback 메서드가 있는 MyErrorObjController로 라우팅
            app.MapFallbackToController("HandleFallback", "MyErrorObj"); // 매칭 안되면 컨트롤러로 라우팅

            app.Run();
        }
    }
}
