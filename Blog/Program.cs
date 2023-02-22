using NLog;
using NLog.Web;

namespace Blog
{
    // ����� ����� � ���������
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("������ ���������� ...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                // ������ ������ � ���
                logger.Error(exception, "Program stopped because of exception");
                throw;
            }
            finally
            {
                // ���������� ������� ����� ������� �� ����������
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
