using LaptopStore.Core.Constants;
using LaptopStore.Data.Context;
using LaptopStore.Services;
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
            /*builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));*/
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

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}");

            app.Run();
        }
    }
}