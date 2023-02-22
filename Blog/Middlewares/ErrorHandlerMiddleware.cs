namespace Blog.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                httpContext.Response.Redirect("/Error/Err");
            }
        }
    }

    // Метод расширения добавления данного промежуточного ПО в конвейер обработки запроса
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
