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
using LaptopStore.Data.ModelDTO.Receipt;
using LaptopStore.Data.ModelDTO.WarehouseExport;
using Microsoft.EntityFrameworkCore.Storage;

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

        public async Task<int> SaveReceipt(ReceiptSaveDTO receipt)
        {
            int result = 1;
            using var transaction = context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("CreateReceipt");
                var importReceipt = Mapper.MapInit<ReceiptSaveDTO, Receipt>(receipt);
                importReceipt.Username = "admin";
                importReceipt.ReceiptDetails = receipt.Products.Select(f => new ReceiptDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    ReceiptId = importReceipt.Id,
                    ProductId = f.Id,
                    UnitPrice = f.UnitPrice,
                    Quantity = f.Quantity
                }).ToList();

                var success = await AddEntityAsync(importReceipt);
                if (success != null)
                {
                    result = 1;
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                result = 0;
                transaction.RollbackToSavepoint("CreateReceipt");
            }
            return result;
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

            using var transaction = context.Database.BeginTransaction();
            try
            {
                var receiptDetails = context.Set<ReceiptDetail>().Where(f => f.ReceiptId == product.Id);
                context.Set<ReceiptDetail>().RemoveRange(receiptDetails);
                await context.SaveChangesAsync();
                var res = await DeleteEntityAsync(product);
                transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                transaction.Rollback(); 
                return 0;
            }
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
