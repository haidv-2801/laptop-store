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
            return await _dbContext.Set<Account>().OrderByDescending(e=>e.Username).ToListAsync();
        }
        public async Task<int> Create(Account account)
        {
            account.Id = Guid.NewGuid().ToString();
            _dbContext.Add<Account>(account);
            int rowAffect =await _dbContext.SaveChangesAsync();
            return rowAffect;
        }
        public async Task<int> Delete(string id)
        {
            int rowAffect = 0;
            var entityToDelete = _dbContext.Find<Account>(id); // Tìm đối tượng cần xóa
            if (entityToDelete != null)
            {
                _dbContext.Remove<Account>(entityToDelete); // Xóa đối tượng
                rowAffect = await _dbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            return rowAffect;
        }
    }
}
