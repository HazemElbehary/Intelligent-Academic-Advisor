using FCAI.Application.Abstraction.Exceptions;
using System.Net;
using System.Text.Json;

namespace FCAI.APIs.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    _logger.LogError(ex, ex.Message);
                }
                else
                {
                    // Log Error In DB Or In File
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            int statusCode;
            string message;
            string? details = null;

            if (ex is ApiExceptionResponse apiEx)
            {
                statusCode = apiEx.StatusCode;
                message = apiEx.Message ?? "";
                details = apiEx.Details ?? "";
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                //message = _environment.IsDevelopment()
                //             ? ex.Message
                //             : "An unexpected error occurred.";
                message = ex.Message;
            }

            // 3) build a simple payload
            var payload = new
            {
                StatusCode = statusCode,
                Message = message,
                Details = details    // will be null in normal exceptions
            };

            var json = JsonSerializer.Serialize(payload);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(json);
        }
    }
}
