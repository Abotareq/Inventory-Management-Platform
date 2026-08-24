using Inventory_Management_Platform.Api.Middlewares;

namespace Inventory_Management_Platform.Api.Extensions
{
    public static class CorrelationIdLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationIdLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdLoggingMiddleware>();
        }
    }
}
