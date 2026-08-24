namespace Inventory_Management_Platform.Api.Middlewares
{
    public sealed class CorrelationIdLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdLoggingMiddleware> _logger;

        public CorrelationIdLoggingMiddleware(
            RequestDelegate next,
            ILogger<CorrelationIdLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["TraceId"] = context.TraceIdentifier
            }))
            {
                await _next(context);
            }
        }
    }
}
