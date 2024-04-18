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
using Microsoft.EntityFrameworkCore.Design.Internal;
using System.Diagnostics;
using System.Data.SqlClient;

namespace LaptopStore.Services.Services.ReceiptService
{
    public class ReceiptService : BaseService<Receipt>, IReceiptService
    {
        protected readonly DbSet<Supplier> dbSupplierSet;
        public ReceiptService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            dbSupplierSet = context.Set<Supplier>();
            _PrefixEntityCode = "DN";
        }

        public async Task<List<Receipt>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Receipt> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }

        private async Task<bool> SaveToProductTable(ICollection<ReceiptDetail> rDetails)
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
            await context.SaveChangesAsync();

            //Lưu số lượng vào kho
            var positions = (from pos in context.Set<Position>()
                            join prod in context.Set<Product>() on pos.Id equals prod.PositionId
                            where listIds.Contains(prod.Id) select pos);

            var allProduct = context.Set<Product>().AsNoTracking().Where(p => p.IsDeleted != true).ToList();
            foreach (var position in positions)
            {
                position.Quantity = allProduct.Where(p => p.PositionId == position.Id).Sum(p => p.Quantity);
            }
            context.Set<Position>().UpdateRange(positions);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveReceipt(ReceiptSaveDTO receipt)
        {
            int result = 1;
            using var transaction = context.Database.BeginTransaction();
            try
            {
                transaction.CreateSavepoint("CreateReceipt");

                var importReceipt = Mapper.MapInit<ReceiptSaveDTO, Receipt>(receipt);
                importReceipt.Id = Guid.NewGuid().ToString();
                importReceipt.Code = await GetNextEntityCode("Code");
                importReceipt.Username = GetUserLoginName();
                AsyncLocalLogger.Log("Id đơn nhập", importReceipt.Id);

                importReceipt.ReceiptDetails = receipt.Products.Select(f => new ReceiptDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    ReceiptId = importReceipt.Id,
                    ProductId = f.Id,
                    UnitPrice = f.UnitPrice,
                    Quantity = f.Quantity
                }).ToList();

                var success = await AddEntityAsync(importReceipt);
                AsyncLocalLogger.Log("Thêm đơn nhập thành công không?", success);

                if (success != null)
                {
                    result = 1;
                }

                //Nếu status là hoàn thành ms lưu vào bảng hàng hóa
                if(result == 1 && receipt.Status == (int)ReceiptStatus.Completed)
                {
                    AsyncLocalLogger.Log("Có lưu vào table product không ?", success);
                    AsyncLocalLogger.Log("Data lưu vào table product", importReceipt.ReceiptDetails);
                    result = await SaveToProductTable(importReceipt.ReceiptDetails) ? 1 : 0;
                }

                if(result == 1) transaction.Commit();
                else transaction.RollbackToSavepoint("CreateReceipt");
            }
            catch (Exception ex)
            {
                result = 0;
                transaction.RollbackToSavepoint("CreateReceipt");
            }
            return result;
        }

        public async Task<bool> UpdateReceipt(string id, ReceiptSaveDTO receipt)
        {
            var rec = await GetEntityByIDAsync(id);
            if (rec == null)
            {
                return false;
            }
                
            var receiptDetailSet = context.Set<ReceiptDetail>();
            var receiptDetails = receiptDetailSet.Where(f => f.ReceiptId == rec.Id).ToList();

            const string SAVE_POINT = "UpdateReceipt";
            using var trans = context.Database.BeginTransaction();

            try
            {
                trans.CreateSavepoint(SAVE_POINT);

                //mapping data lưu sang
                Mapper.MapUpdate(receipt, rec);

                //update và delete
                var deletes = new List<ReceiptDetail>();
                foreach (var item in receiptDetails)
                {
                    var product = receipt.Products.Find(f => f.Id == item.ProductId);

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
                receiptDetailSet.UpdateRange(receiptDetails);
                receiptDetailSet.RemoveRange(deletes);
                await context.SaveChangesAsync();

                //add
                foreach (var prodAdd in receipt.Products)
                {
                    var founded = receiptDetails.FirstOrDefault(f => f.ProductId == prodAdd.Id);
                    if (founded == null)
                    {
                        receiptDetailSet.Add(new ReceiptDetail
                        {
                            Id = Guid.NewGuid().ToString(),
                            ReceiptId = rec.Id,
                            ProductId = prodAdd.Id,
                            UnitPrice = prodAdd.UnitPrice,
                            Quantity = prodAdd.Quantity,
                        });
                        context.SaveChanges();
                    }
                }
                
                rec.Username = GetUserLoginName();
                var result = await UpdateEntityAsync(rec);

                //nếu comlete thì lưu vào hàng hóa
                if(receipt.Status == (int)ReceiptStatus.Completed)
                {
                    result = await SaveToProductTable(receiptDetailSet.Where(f => f.ReceiptId == rec.Id).ToList()) ? 1 : 0;
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

            var data = result.Data.ToList();
            for (int i = 0; i < data.Count(); i++)
            {
                var supplier = await dbSupplierSet.AsNoTracking().FirstOrDefaultAsync(supplier => supplier.Id == data[i].SupplierId);
                data[i].Supplier = supplier;
            }
            pagingResponse.Data = data;
            pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
