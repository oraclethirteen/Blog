namespace Blog.Middlewares
{
   
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string userName = string.IsNullOrEmpty(httpContext.User.Identity.Name) ? "Anonymous" : httpContext.User.Identity.Name;
            
            _logger.LogInformation($"Метод: {httpContext.Request.Method}; Путь: {httpContext.Request.Path}; Пользователь: {userName};");
            await _next.Invoke(httpContext);
        }

    }

    // Метод расширения добавления данного промежуточного ПО в конвейер обработки запроса
    public static class LogExtensions
    {
        public static IApplicationBuilder UseLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
