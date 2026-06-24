using System.Diagnostics;

namespace DebtSettlementService.API.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();
        logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
