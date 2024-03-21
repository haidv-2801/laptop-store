using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Account>> GetAll()
        {
            return await _dbContext.Set<Account>().ToListAsync();
        }

        public async Task<string> SaveAccount(AccountSaveDTO accountSaveDTO)
        {
            var account = Mapper.MapInit<AccountSaveDTO, Account>(accountSaveDTO);
            _dbContext.Accounts.Add(account);
            int result = await _dbContext.SaveChangesAsync();
            return result.ToString();
        }

        public async Task<bool> UpdateAccount(string id, AccountSaveDTO accountSaveDTO)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                return false;

            account.FullName = accountSaveDTO.FullName;
            
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccount(string id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                return false;

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
