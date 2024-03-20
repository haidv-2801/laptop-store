using LaptopStore.Services.Services.AccountService;
using Microsoft.Extensions.DependencyInjection;

namespace LaptopStore.Services
{
    public class ServiceInjectionExtension
    {
        public static void InjectService(IServiceCollection collections)
        {
            collections.AddTransient<IAccountService, AccountService>();
        }
    }
}
