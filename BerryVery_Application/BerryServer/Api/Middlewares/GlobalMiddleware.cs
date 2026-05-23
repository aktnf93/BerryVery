using System.Text.Json;

namespace BerryServer.Api.Middlewares
{
    public class GlobalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalMiddleware> _logger;

        public GlobalMiddleware(RequestDelegate next, ILogger<GlobalMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 공통 체크 로직
                var path = context.Request.Path;
                this._logger.LogInformation("GlobalMiddleware.InvokeAsync");

                // 예: 특정 조건 차단
                if (path.StartsWithSegments("/blocked"))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("접근 차단");
                    return;
                }


                await this._next(context); // 다음 파이프라인 (Middleware or Controller)
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Unhandled Exception");

                await GlobalMiddleware.HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                success = false,
                message = "서버 오류가 발생했습니다.",
                detail = ex.Message
            };

            // var json = JsonSerializer.Serialize(response);
            // await context.Response.WriteAsync(response);

            await TypedResults.Json(response).ExecuteAsync(context);
        }
    }
}
