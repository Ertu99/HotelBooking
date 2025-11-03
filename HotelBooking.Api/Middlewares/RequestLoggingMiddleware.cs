using Serilog;

namespace HotelBooking.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;
            var request = context.Request;

            Log.Information("Handling {method} {path}", request.Method, request.Path);

            await _next(context);

            var elapsed = DateTime.UtcNow - start;
            Log.Information("Handled {method} {path} in {elapsed} ms (Status {status})",
                request.Method, request.Path, elapsed.TotalMilliseconds, context.Response.StatusCode);
        }
    }
}
