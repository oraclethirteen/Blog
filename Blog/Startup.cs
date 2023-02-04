using AutoMapper;
using Blog.DAL;
using Blog.DAL.UoW;
using Blog.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace Blog
{
    public class Startup
    {
        Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _logger.Debug("init main");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Подключение БД
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDbContext>(options => options.UseSqlite(connection), ServiceLifetime.Singleton);

            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            MapperConfiguration mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog", Version = "v1" });
            //});

            services.AddAuthentication(options =>
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContent =>
                        {
                            redirectContent.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

            //services.AddControllers();
            //services.AddEndpointsApiExplorer();

            // Подключение поддержки контроллеров с представлениями
            services.AddControllersWithViews();

            //services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(options =>
                //{
                //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                //    options.RoutePrefix = string.Empty;
                //});
            }
            else
            {
                // Глобальный обработчик исключений
                app.UseErrorHandler();
                // Использование HTTPS (принудительно)
                app.UseHsts();
            }

            // Обработка ошибок HTTP
            //app.UseStatusCodePages();
            //app.UseStatusCodePagesWithRedirects();
            //app.UseStatusCodePagesWithReExecute();

            // Отдача статических файлов клиенту
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Компонент для логирования запросов
            app.Use(async (context, next) =>
            {
                // Применяется свойство объекта HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Article}/{action=ArticleList}/{id?}"
                    );
            });
        }
    }
}
