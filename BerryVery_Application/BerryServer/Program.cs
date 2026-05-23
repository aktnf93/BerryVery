using System.Text.Encodings.Web;
using System.Text.Unicode;

using Serilog;

using BerryServer.Api.Middlewares;
using BerryServer.Application.Repositories;
using BerryServer.Application.Services;
using BerryServer.Infrastructure.Data;
using BerryServer.Infrastructure.Network;

namespace BerryServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // _______________________________________________________________________________
            // dotnet add package Serilog.AspNetCore
            // dotnet add package Serilog.Sinks.File
            // Serilog 설정 (하루 단위로 로그 파일 분할 생성, 최대 10MB 크기 유지)
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Logger.Information("#################### App Start ####################");

            var builder = WebApplication.CreateBuilder(args);

            // 모든 한글(UnicodeRanges.All 또는 .Hangul)을 안전하게 인코딩하도록 설정
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(UnicodeRanges.All));

            // 애플리케이션의 기본 기본 로거를 Serilog로 대체
            builder.Host.UseSerilog();

            // _______________________________________________________________________________
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // 전역 JSON 응답의 날짜 포맷을 원하는 형태로 고정합니다.
                    options.JsonSerializerOptions.Converters.Add(
                        new System.Text.Json.Serialization.JsonStringEnumConverter()); // 필요 시 Enum 처리용

                    // .NET 7 이상 표준 날짜 포맷팅 설정 방식 방식
                    options.JsonSerializerOptions.WriteIndented = false; // 용량 최소화를 위해 들여쓰기 끔
                });

            // _______________________________________________________________________________
            // DI 등록 : HTTP 요청 시 생성자를 통해 내려받을 타입 등록

            builder.Services.AddSingleton<SocketConnection>();
            builder.Services.AddScoped<DatabaseConnection>();
            builder.Services.AddScoped<RoomRepository>();
            builder.Services.AddScoped<RoomService>();
            
            builder.Services.AddControllers();

            // builder.Services.AddHostedService<SocketConnection>(); // ASP.NET 백그라운드 작업

            builder.WebHost.ConfigureKestrel(o =>
            {
                o.ListenAnyIP(8016);
            });

            var app = builder.Build();
            app.UseMiddleware<GlobalMiddleware>(); // 공통 실행 (요청 검사 + 예외 처리)
            app.UseStaticFiles();                  // wwwroot 정적 자원(css, js) 서빙 (프로젝트 내 혹은 같은 실행 경로)
            app.UseRouting();                      // 라우팅 엔진 활성화 필수 추가
            app.MapControllers();                  // 어트리뷰트 라우팅을 사용하여 컨트롤러의 액션 메서드에 직접 URL 매핑
            // app.MapFallbackToFile("index.html");   // URL 매핑 실패 시 wwwroot/index.html 파일을 반환하여 SPA 프론트엔드로 라우팅 (React, Vue etc)

            app.MapFallbackToFile("/app/{*path:nonfile}", "index.html"); // "/app" 경로로 시작하는 모든 요청에 대해 wwwroot/index.html 파일을 반환하여 SPA 프론트엔드로 라우팅 (React, Vue etc)
            app.MapFallbackToController("Default", "Fallback");  // FallbackController의 Default 액션 메서드로 매핑하여 NotFound 응답 반환
            
            app.Run();
        }
    }
}
