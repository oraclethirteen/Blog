using NLog;
using NLog.Web;

namespace Blog
{
    // Точка входа в программу
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("Запуск приложения ...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                // Запись ошибки в лог
                logger.Error(exception, "Program stopped because of exception");
                throw;
            }
            finally
            {
                // Отключение логгера перед выходом из приложения
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                }).UseNLog();
    }
}
