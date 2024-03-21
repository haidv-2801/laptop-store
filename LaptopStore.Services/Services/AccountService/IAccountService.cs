using LaptopStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.AccountService
{
    public interface IAccountService
    {
        Task<List<Account>> GetAll();
        Task<int> Create(Account account);
        Task<int> Delete(string id);
    }
}
