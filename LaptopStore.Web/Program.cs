using LaptopStore.Core.Constants;
using LaptopStore.Data.Context;
using LaptopStore.Services;
using LaptopStore.Services.Services.StorageService;
using LaptopStore.Web.MiddleWare;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LaptopStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Cấu hình Session Middleware
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); // Thời gian timeout của session
                options.Cookie.Name = ".My.Session"; // Tên của cookie session
                                                     // Cấu hình khác nếu cần thiết
            });
            // Đăng ký IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Add cookie authen
            CookiesAuthenication(builder);

            // Register db context
            InitDatabaseConnection(builder);

            //StorageService
            builder.Services.AddScoped<IStorageService>(serviceProvider =>
            {
                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                return new StorageService(env.WebRootPath);
            });

            ServiceInjectionExtension.InjectService(builder.Services);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Chuyển hướng các yêu cầu từ /images đến wwwroot/images
            app.UseRewriter(new RewriteOptions()
                .AddRewrite(@"^images/(.*)", "wwwroot/images/$1", skipRemainingRules: true));

            app.UseRouting();
            // Kích hoạt Session Middleware
            app.UseSession();

            // Đăng ký middleware kiểm tra xác thực
            //app.UseMiddleware<AuthenticationMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            MapControllerRoutes(app);

            app.Run();
        }

        /// <summary>
        /// Map router ở đây
        /// </summary>
        /// <param name="app"></param>
        private static void MapControllerRoutes(WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}");
        }

        /// <summary>
        /// Cookie authen
        /// </summary>
        /// <param name="app"></param>
        private static void CookiesAuthenication(WebApplicationBuilder builder)
        {
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.AccessDeniedPath = new PathString("/Account/Access");
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.Cookie = new CookieBuilder
                {
                    //Domain = "",
                    HttpOnly = true,
                    Name = ".LaptopStore.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events = new CookieAuthenticationEvents
                {
                    OnSignedIn = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnSignedIn", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnSigningOut = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnSigningOut", context.HttpContext.User.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnValidatePrincipal = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnValidatePrincipal", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    }
                };
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = new PathString("/Auth/Login");
                options.ReturnUrlParameter = "RequestPath";
                options.SlidingExpiration = true;
            });
        }

        private static void InitDatabaseConnection(WebApplicationBuilder builder)
        {
            // Đọc cài đặt từ file config và thêm vào Configuration
            builder.Configuration.AddJsonFile("dbconfig.json", optional: true, reloadOnChange: true);

            // Register db context
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString(Constant.ConnectionStringKey));
            /*var connectionString = builder.Configuration.GetConnectionString(Constant.ConnectionStringKey);*/
            if (!string.IsNullOrEmpty(builder.Configuration.GetValue<string>("ServerName")))
            {
                connectionBuilder.DataSource = builder.Configuration.GetValue<string>("ServerName");
            }
            if (!string.IsNullOrEmpty(builder.Configuration.GetValue<string>("DatabaseName")))
            {
                connectionBuilder.InitialCatalog = builder.Configuration.GetValue<string>("DatabaseName");
            }
            if (!string.IsNullOrEmpty(builder.Configuration.GetValue<string>("UserId")))
            {
                connectionBuilder.UserID = builder.Configuration.GetValue<string>("UserId");
            }
            if (!string.IsNullOrEmpty(builder.Configuration.GetValue<string>("Password")))
            {
                connectionBuilder.Password = builder.Configuration.GetValue<string>("Password");
            }
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionBuilder.ConnectionString));
        }
    }
}