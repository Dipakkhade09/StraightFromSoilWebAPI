namespace KhadeFarm_Web_API.CustomMiddleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            // Log the incoming request
            _logger.LogInformation("Incoming Request: {Method} {Path}", context.Request.Method, context.Request.Path);
            // Call the next middleware in the pipeline
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
           

            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;

            // Log the outgoing response
            _logger.LogWarning("Response Status: {StatusCode}", context.Response.StatusCode);
            _logger.LogError("Execution Time: {Duration} ms", duration.TotalMilliseconds);
        }
    }
}
