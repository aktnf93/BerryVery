using System.Text.Json;

namespace BerryServer.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context); // 다음 미들웨어 또는 Controller 실행
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Unhandled Exception");

                await GlobalExceptionMiddleware.HandleExceptionAsync(context, ex);
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
