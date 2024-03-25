using LaptopStore.Services.Services.AccountService;
using LaptopStore.Services.Services.AuthService;
using LaptopStore.Services.Services.BaseService;
using LaptopStore.Services.Services.CustomerService;
using LaptopStore.Services.Services.PositionService;
using LaptopStore.Services.Services.ProductCategoryService;
using LaptopStore.Services.Services.ProductService;
using LaptopStore.Services.Services.SupplierService;
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
            collections.AddTransient<IProductService, ProductService>();
            collections.AddTransient<IProductCategoryService, ProductCategoryService>();
            collections.AddTransient<ISupplierService, SupplierService>();
            collections.AddTransient<ICustomerService, CustomerService>();
        }
    }
}
