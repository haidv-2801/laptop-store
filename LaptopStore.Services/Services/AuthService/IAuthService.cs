using LaptopStore.Core;
using LaptopStore.Data.ModelDTO;
using LaptopStore.Data.Models;
using LaptopStore.Services.Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.AuthService
{
    public interface IAuthService : IBaseService<Account>
    {
        public Task<ServiceResponse> SignIn(AccountLoginDTO accountLoginDTO);
        public Task<ServiceResponse> SignOut();
    }
}
