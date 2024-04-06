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
using LaptopStore.Services.Services.PositionService;
using System.Reflection;

namespace LaptopStore.Services.Services.WarehouseExportService
{
    public class WarehouseExportService : BaseService<WarehouseExport>, IWarehouseExportService
    {
        protected readonly DbSet<Product> dbProductSet;
        protected readonly DbSet<Position> dbPositionSet;
        protected readonly DbSet<Customer> dbCustomerSet;
        protected readonly IProductService _productService;
        protected readonly IPositionService _positionService;
        public WarehouseExportService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IProductService productService, IPositionService positionService) : base(dbContext, httpContextAccessor)
        {
            dbProductSet = context.Set<Product>();
            dbPositionSet = context.Set<Position>();
            dbCustomerSet = context.Set<Customer>();
            _productService = productService;
            _positionService = positionService;
        }

        public async Task<List<WarehouseExport>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<WarehouseExport> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }

        public async Task<WarehouseExport> GetByIDIncludesDetail(string id)
        {
            return await dbSet.Include(e => e.WarehouseExportDetails).FirstOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<int> SaveWarehouseExport(WarehouseExportSaveDTO warehouseExportSaveDTO)
        {
            int result = 1;
            using var transaction = context.Database.BeginTransaction();
            try
            {

                transaction.CreateSavepoint("CreateWarehouseExport");
                var warehouseExport = Mapper.MapInit<WarehouseExportSaveDTO, WarehouseExport>(warehouseExportSaveDTO);
                var value = _httpContextAccessor.HttpContext.Request.Cookies["UserLogin"];
                if (value != null)
                {
                    var account = JsonConvert.DeserializeObject<Account>(value);
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

                warehouseExport.Code = await GetNextEntityCode();
                var success = await AddEntityAsync(warehouseExport);
                if (success != null)
                {
                    List<Product> products = new List<Product>();
                    List<Position> positions = new List<Position>();
                    //Nếu trạng thái đơn là hoàn thành thì update số lượng cho product và position
                    if (warehouseExport.Status == (int)WarehouseExportStatus.Completed)
                    {
                        List<WarehouseExportDetail> details = warehouseExport.WarehouseExportDetails.ToList();
                        for (int i = 0; i < details.Count(); i++)
                        {
                            var product = await _productService.GetById(details[i].ProductId);
                            product.Quantity -= details[i].Quantity;

                            products.Add(product);

                            var position = await dbPositionSet.FindAsync(product.PositionId);

                            position.Quantity -= details[i].Quantity;
                            positions.Add(position);
                        }
                        dbProductSet.UpdateRange(products);
                        result = await context.SaveChangesAsync() != 0 ? 1 : 0;

                        if (result == 1)
                        {
                            dbPositionSet.UpdateRange(positions);
                            result = await context.SaveChangesAsync() != 0 ? 1 : 0;
                        }
                    }
                }
                else
                {
                    result = 0;
                }
                if (result == 1) transaction.Commit();
            }
            catch (Exception ex)
            {
                result = 0;
                transaction.RollbackToSavepoint("CreateWarehouseExport");
            }
            return result;
        }

        public async Task<bool> UpdateWarehouseExport(string id, WarehouseExportSaveDTO warehouseExportSaveDTO)
        {
            var warehouseExport = await GetEntityByIDAsync(id);
            if (warehouseExport == null)
                return false;

            var warehouseExportDetailSet = context.Set<WarehouseExportDetail>();
            var warehouseExportDetails = warehouseExportDetailSet.Where(f => f.WarehouseExportId == warehouseExport.Id).ToList();

            const string SAVE_POINT = "UpdateWarehouseExport";
            using var trans = context.Database.BeginTransaction();

            try
            {
                trans.CreateSavepoint(SAVE_POINT);

                //mapping data lưu sang
                Mapper.MapUpdate(warehouseExportSaveDTO, warehouseExport);

                //update và delete
                var deletes = new List<WarehouseExportDetail>();
                foreach (var item in warehouseExportDetails)
                {
                    var product = warehouseExportSaveDTO.Products.Find(f => f.Id == item.ProductId);

                    //cập nhật vào cũ
                    if (product != null)
                    {
                        item.Quantity = product.Quantity;
                        item.UnitPrice = product.UnitPrice;
                    }
                    //Xóa
                    else
                    {
                        deletes.Add(item);
                    }
                }
                warehouseExportDetailSet.UpdateRange(warehouseExportDetails);
                warehouseExportDetailSet.RemoveRange(deletes);
                context.SaveChanges();

                //add
                foreach (var prodAdd in warehouseExportSaveDTO.Products)
                {
                    var founded = warehouseExportDetails.FirstOrDefault(f => f.ProductId == prodAdd.Id);
                    if (founded == null)
                    {
                        warehouseExportDetailSet.Add(new WarehouseExportDetail
                        {
                            Id = Guid.NewGuid().ToString(),
                            WarehouseExportId = warehouseExport.Id,
                            ProductId = prodAdd.Id,
                            UnitPrice = prodAdd.UnitPrice,
                            Quantity = prodAdd.Quantity,
                        });
                        context.SaveChanges();
                    }
                }

                var result = await UpdateEntityAsync(warehouseExport);

                //nếu comlete thì lưu vào hàng hóa
                if (result != 0)
                {
                    List<Product> products = new List<Product>();
                    List<Position> positions = new List<Position>();
                    //Nếu trạng thái đơn là hoàn thành thì update số lượng cho product và position
                    if (warehouseExportSaveDTO.Status == (int)WarehouseExportStatus.Completed)
                    {
                        List<WarehouseExportDetail> details = warehouseExport.WarehouseExportDetails.ToList();
                        for (int i = 0; i < details.Count(); i++)
                        {
                            var product = await _productService.GetById(details[i].ProductId);
                            product.Quantity -= details[i].Quantity;

                            products.Add(product);

                            var position = await dbPositionSet.FindAsync(product.PositionId);

                            position.Quantity -= details[i].Quantity;
                            positions.Add(position);
                        }
                        dbProductSet.UpdateRange(products);
                        result = await context.SaveChangesAsync() != 0 ? 1 : 0;

                        if (result == 1)
                        {
                            dbPositionSet.UpdateRange(positions);
                            result = await context.SaveChangesAsync() != 0 ? 1 : 0;
                        }
                    }
                }
                else
                {
                    result = 0;
                }

                if (result != 0)
                {
                    trans.Commit();
                }
                else { trans.RollbackToSavepoint(SAVE_POINT); }
                return result != 0;
            }
            catch (Exception)
            {
                trans.RollbackToSavepoint(SAVE_POINT);
                throw;
            }
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
            var data = result.Data.ToList();
            for (int i=0; i< data.Count();i++)
            {
                var customer = await dbCustomerSet.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == data[i].CustomerId);
                data[i].Customer = customer;
            }
            pagingResponse.Data = data;
            pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }

        private async Task<bool> SaveToProductAndPositionTable(ICollection<ReceiptDetail> rDetails)
        {
            var listIds = rDetails.Select(f => f.ProductId);
            // Lưu vào hàng hóa
            var products = context.Set<Product>().Where(p => listIds.Contains(p.Id)).ToList();

            foreach (var product in products)
            {
                var productReceipt = rDetails.First(f => f.ProductId == product.Id);
                if (productReceipt != null)
                {
                    //product.UnitPrice = productReceipt.UnitPrice;
                    product.Quantity = (product.Quantity ?? 0) + (productReceipt.Quantity ?? 0);
                }
            }
            context.Set<Product>().UpdateRange(products);
            return await context.SaveChangesAsync() != 0;
        }
    }
}
