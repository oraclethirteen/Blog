using Blog.DAL;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.UoW;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog;
using NLog.Web;
using Blog.Middlewares;
using Blog.BLL.Services;

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

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //Подключение БД
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDbContext>(options => options.UseSqlite(connection), ServiceLifetime.Singleton);

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IArticleService, ArticleService>();
            services.AddSingleton<ICommentService, CommentService>();
            services.AddSingleton<ITagService, TagService>();
            services.AddSingleton<IRoleService, RoleService>();

            MapperConfiguration mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);


            services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirctContent =>
                        {
                            redirctContent.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

            // Подключение поддержки контроллеров с представлениями
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Активация промежуточного ПО логирования запросов
            app.UseLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Глобальный обработчик исключений
                app.UseErrorHandler();
                // Использование HTTPS (принудительное)
                app.UseHsts();
            }

            // Отдача статических файлов клиенту
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Компонент для логирования запросов
            app.Use(async (context, next) =>
            {
                // Применение свойства объекта HttpContext
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
