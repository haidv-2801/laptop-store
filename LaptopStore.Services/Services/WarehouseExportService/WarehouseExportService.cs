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
using LaptopStore.Data.ModelDTO.WarehouseExport;
using LaptopStore.Data.ModelDTO.ProductCategory;

namespace LaptopStore.Services.Services.WarehouseExportService
{
    public class WarehouseExportService : BaseService<WarehouseExport>, IWarehouseExportService
    {
        public WarehouseExportService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<List<WarehouseExport>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<WarehouseExport> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }

        public async Task<int> SaveWarehouseExport(WarehouseExportSaveDTO warehouseExportSaveDTO)
        {
            int result = 1;
            using var transaction = context.Database.BeginTransaction();
            try
            {

                transaction.CreateSavepoint("CreateWarehouseExport");
                _httpContextAccessor.HttpContext.Session.TryGetValue("UserLogin", out byte[] value);
                var warehouseExport = Mapper.MapInit<WarehouseExportSaveDTO, WarehouseExport>(warehouseExportSaveDTO);
                if (value != null)
                {
                    var account = JsonConvert.DeserializeObject<Account>(Encoding.UTF8.GetString(value));
                    warehouseExport.Username = account.Username;
                }
                warehouseExport.WarehouseExportDetails = warehouseExportSaveDTO.Products.Select(f => new WarehouseExportDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    WarehouseExportId = warehouseExport.Id,
                    ProductId = f.Id,
                    UnitPrice = f.UnitPrice,
                    Quantity = f.Quantity
                }).ToList();

                var success = await AddEntityAsync(warehouseExport);
                if(success != null)
                {
                    result = 1;
                }
                transaction.Commit();
            }
            catch(Exception ex)
            {
                result = 0;
                transaction.RollbackToSavepoint("CreateWarehouseExport");
            }
            return result;
        }

        public async Task<bool> UpdateWarehouseExport(string id, WarehouseExport receipt)
        {
            var rec = await GetEntityByIDAsync(id);
            if (rec == null)
                return false;


            await UpdateEntityAsync(rec);
            return true;
        }

        public async Task<int> DeleteWarehouseExport(string id)
        {
            var warehouseExport = await GetEntityByIDAsync(id);
            if (warehouseExport == null)
                return 0;

            using var transaction = context.Database.BeginTransaction();
            try
            {
                var warehouseExportDetails = context.Set<ReceiptDetail>().Where(f => f.ReceiptId == warehouseExport.Id);
                context.Set<ReceiptDetail>().RemoveRange(warehouseExportDetails);
                await context.SaveChangesAsync();
                var res = await DeleteEntityAsync(warehouseExport);
                transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }
        }

        public async Task<PagingResponse> GetWarehouseExportPaging(PagingRequest paging)
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
