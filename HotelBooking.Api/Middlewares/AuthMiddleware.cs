using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelBooking.Api.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        _logger.LogInformation("🔥🔥🔥 AuthMiddleware tetiklendi → Path: {path}", context.Request.Path);

        try
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            // 🔥 1) Header yoksa direkt log yaz
            if (string.IsNullOrEmpty(authHeader))
            {
                _logger.LogWarning("No Authorization header found for request {path}", context.Request.Path);
                await _next(context);
                return;
            }

            // 🔥 2) Token çıkar
            var token = authHeader.StartsWith("Bearer ")
                ? authHeader.Substring("Bearer ".Length)
                : null;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Authorization header present but no Bearer token (path: {path})", context.Request.Path);
                await _next(context);
                return;
            }

            // 🔥 3) Token parse edilmeye çalışılır
            var handler = new JwtSecurityTokenHandler();

            // ReadJwtToken sadece token parse eder, doğrulamaz
            var jwt = handler.ReadJwtToken(token);

            var email = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            _logger.LogInformation("CustomAuth: Email={email}, Role={role}, Path={path}",
                email, role, context.Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuthMiddleware failed for path {path}", context.Request.Path);
            // 🔥 Hata olsa bile pipeline devam etmeli
        }

        await _next(context);
    }
}
