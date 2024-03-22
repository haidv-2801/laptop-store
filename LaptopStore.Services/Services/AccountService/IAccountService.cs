using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
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
        Task<Account> GetById(string id);

        Task<int> SaveAccount(AccountSaveDTO accountSaveDTO);

        Task<bool> UpdateAccount(string id, AccountSaveDTO accountSaveDTO);

        Task<int> DeleteAccount(string id);
    }
}
