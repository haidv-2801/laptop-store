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
using LaptopStore.Core;
using System.Net.Http.Json;
using Newtonsoft.Json;
using LaptopStore.Core.Enums;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.AccountService
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        public AccountService(ApplicationDbContext dbContext):base(dbContext)
        {
        }

        public async Task<List<Account>> GetAll()
        {
            return await dbSet.OrderByDescending(e => e.Username).ToListAsync();
        }

        public async Task<Account> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }
        public async Task<int> SaveAccount(AccountSaveDTO accountSaveDTO)
        {
            var account = Mapper.MapInit<AccountSaveDTO, Account>(accountSaveDTO);
            await AddEntityAsync(account);
            return 1;
        }

        public async Task<bool> UpdateAccount(string id, AccountSaveDTO accountSaveDTO)
        {
            var account = await GetEntityByIDAsync(id);
            if (account == null)
                return false;
            account.FullName = accountSaveDTO.FullName;
            account.Password = accountSaveDTO.Password;
            account.Gender = accountSaveDTO.Gender;
            account.Address = accountSaveDTO.Address;
            account.AccountType = accountSaveDTO.AccountType;
            account.Username = accountSaveDTO.Username;
            await UpdateEntityAsync(account);
            return true;
        }

        public async Task<int> DeleteAccount(string id)
        {
            var account = await GetEntityByIDAsync(id);
            if (account == null)
                return 0;

            return await DeleteEntityAsync(account);
        }

        public async Task<PagingResponse> GetAccountPaging(PagingRequest paging)
        {
            int skip = (paging.Page - 1) * paging.PageSize;
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //Search
            var search = dbSet.Where(f =>
                f.FullName.ToLower().Contains(paging.Search.ToLower())
                || f.Username.ToLower().Contains(paging.Search.ToLower()));

            //Sort
            if (!string.IsNullOrEmpty(paging.Sort))
            {
                var sortArr = JsonConvert.DeserializeObject<List<string>>(paging.Sort);
                foreach (var sort in sortArr)
                {
                    string fieldName = sort.Split(":")[0];
                    string sortType = sort.Split(":")[1];
                    //làm sau
                }
            }

            //Paing
            pagingResponse.Total = search.Count();
            pagingResponse.Data = search.Skip(skip).Take(paging.PageSize).ToList();

            return pagingResponse;
        }
    }
}
