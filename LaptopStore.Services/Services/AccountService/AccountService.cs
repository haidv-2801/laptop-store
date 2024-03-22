using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
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
            return await _dbContext.Set<Account>().OrderByDescending(e=>e.Username).ToListAsync();
        }

        public async Task<Account> GetById(string id)
        {
            var account = _dbContext.Accounts.SingleOrDefault(e=>e.Id == id);
            return account;
        }
        public async Task<int> SaveAccount(AccountSaveDTO accountSaveDTO)
        {
            var account = Mapper.MapInit<AccountSaveDTO, Account>(accountSaveDTO);
            _dbContext.Accounts.Add(account);
            int result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateAccount(string id, AccountSaveDTO accountSaveDTO)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                return false;
            account.FullName = accountSaveDTO.FullName;
            account.Password = accountSaveDTO.Password;
            account.Gender = accountSaveDTO.Gender;
            account.Address = accountSaveDTO.Address;
            account.AccountType = accountSaveDTO.AccountType;
            account.Username = accountSaveDTO.Username;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteAccount(string id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                return 0;

            _dbContext.Accounts.Remove(account);
            int result = await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}
