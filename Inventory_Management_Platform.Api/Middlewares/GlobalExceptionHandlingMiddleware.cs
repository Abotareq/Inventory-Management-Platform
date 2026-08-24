using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Inventory_Management_Platform.Api.Middlewares
{

    public sealed class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
                    traceId, context.Request.Path, context.Request.Method);

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "An unexpected error occurred. Please try again later.",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    Extensions = { ["traceId"] = traceId }
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
