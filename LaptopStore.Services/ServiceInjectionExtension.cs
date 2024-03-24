using LaptopStore.Services.Services.AccountService;
using LaptopStore.Services.Services.AuthService;
using LaptopStore.Services.Services.BaseService;
using LaptopStore.Services.Services.PositionService;
using Microsoft.Extensions.DependencyInjection;

namespace LaptopStore.Services
{
    public class ServiceInjectionExtension
    {
        public static void InjectService(IServiceCollection collections)
        {
            collections.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            collections.AddTransient<IAccountService, AccountService>();
            collections.AddTransient<IPositionService, PositionService>();
            collections.AddTransient<IAuthService, AuthService>();
        }
    }
}
