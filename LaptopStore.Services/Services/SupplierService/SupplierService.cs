using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.Supplier;
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

namespace LaptopStore.Services.Services.SupplierService
{
    public class SupplierService : BaseService<Supplier>, ISupplierService
    {
        public SupplierService(ApplicationDbContext dbContext):base(dbContext)
        {
        }

        public async Task<List<Supplier>> GetAll()
        {
            return await dbSet.OrderByDescending(e => e.Name).ToListAsync();
        }

        public async Task<Supplier> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }
        public async Task<int> SaveSupplier(SupplierSaveDTO supplierSaveDTO)
        {
            var supplier = Mapper.MapInit<SupplierSaveDTO, Supplier>(supplierSaveDTO);
            await AddEntityAsync(supplier);
            return 1;
        }

        public async Task<bool> UpdateSupplier(string id, SupplierSaveDTO supplierSaveDTO)
        {
            var supplier = await GetEntityByIDAsync(id);
            if (supplier == null)
                return false;
            supplier.Name = supplierSaveDTO.Name;
            supplier.ContactName = supplierSaveDTO.ContactName;
            supplier.Email = supplierSaveDTO.Email;
            supplier.Phone = supplierSaveDTO.Phone;
            await UpdateEntityAsync(supplier);
            return true;
        }

        public async Task<int> DeleteSupplier(string id)
        {
            var supplier = await GetEntityByIDAsync(id);
            if (supplier == null)
                return 0;

            return await DeleteEntityAsync(supplier);
        }

        public async Task<bool> CheckDuplicateName(string name)
        {
            var supplier = context.Set<Supplier>().AsNoTracking().FirstOrDefault(e => e.Name == name);
            if (supplier == null)
                return false;

            return true;
        }

        public async Task<bool> CheckDuplicateNameNotThis(string id, string name)
        {
            var position = context.Set<Position>().AsNoTracking().FirstOrDefault(e => e.Name == name && e.Id != id);
            if (position == null)
                return false;

            return true;
        }

        public async Task<PagingResponse> GetSupplierPaging(PagingRequest paging)
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
