namespace BerryServer.Middleware
{
    public class RequestCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 공통 체크 로직
            var path = context.Request.Path;

            Console.WriteLine("{0} > 미들웨어 > 요청 경로: {1}", DateTime.Now, path);

            // 예: 특정 조건 차단
            if (path.StartsWithSegments("/blocked"))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("접근 차단");
                return;
            }

            await _next(context); // 다음 파이프라인
        }
    }
}
