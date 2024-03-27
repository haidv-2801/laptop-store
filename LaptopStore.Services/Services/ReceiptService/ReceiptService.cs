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
using LaptopStore.Services.Services.ProductService;
using Microsoft.AspNetCore.Http;

namespace LaptopStore.Services.Services.ReceiptService
{
    public class ReceiptService : BaseService<Receipt>, IReceiptService
    {
        public ReceiptService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<List<Receipt>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Receipt> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }

        public async Task<int> SaveReceipt(Receipt receipt)
        {
            var success = await AddEntityAsync(receipt);
            return success != null ? 1 : 0;
        }

        public async Task<bool> UpdateReceipt(string id, Receipt receipt)
        {
            var rec = await GetEntityByIDAsync(id);
            if (rec == null)
                return false;


            await UpdateEntityAsync(rec);
            return true;
        }

        public async Task<int> DeleteReceipt(string id)
        {
            var product = await GetEntityByIDAsync(id);
            if (product == null)
                return 0;

            return await DeleteEntityAsync(product);
        }

        public async Task<PagingResponse> GetReceiptPaging(PagingRequest paging)
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
