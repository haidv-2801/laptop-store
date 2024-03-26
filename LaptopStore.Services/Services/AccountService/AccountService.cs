using LaptopStore.Core.Utilities;
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
using LaptopStore.Data.Context;
using Microsoft.AspNetCore.Http;

namespace LaptopStore.Services.Services.AccountService
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        public AccountService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
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
            accountSaveDTO.Password = Hasher.MD5(accountSaveDTO.Password);
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
            account.Password = Hasher.MD5(accountSaveDTO.Password); ;
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
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //f => true có thể sửa theo nghiệp vụ ví dụ như f.Status = true;
            var result = await FilterEntitiesPagingAsync(f => true, paging.Search, paging.SearchField, paging.Sort, paging.Page, paging.PageSize);
                pagingResponse.Data = result.Data;
                pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
