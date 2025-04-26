namespace CzechUp.WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";

                var (statusCode, message) = GetErrorResponse(ex);

                context.Response.StatusCode = statusCode;

                var response = new
                {
                    error = message,
                    code = statusCode
                };

                var json = System.Text.Json.JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private (int StatusCode, string Message) GetErrorResponse(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Required argument was null."),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
                InvalidOperationException => (StatusCodes.Status409Conflict, "Invalid operation."),
                _ when ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) =>
                    (StatusCodes.Status504GatewayTimeout, "Request timed out."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };
        }
    }
}
