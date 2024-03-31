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
using Microsoft.AspNetCore.Http;

namespace LaptopStore.Services.Services.ProductService
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<List<Product>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }

        public async Task<int> SaveProduct(ProductSaveDTO productSaveDTO)
        {
            var product = Mapper.MapInit<ProductSaveDTO, Product>(productSaveDTO);
            var success = await AddEntityAsync(product);

            //Thêm số lượng cho bên position
            var position = context.Set<Position>().Find(product.PositionId);
            if(position != null) 
            {
                position.Quantity = (position.Quantity ?? 0) + (product.Quantity ?? 0);
                context.Set<Position>().Attach(position);
                context.Entry(position).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return success != null ? 1 : 0;
        }

        public async Task<bool> UpdateProduct(string id, ProductSaveDTO productSaveDTO)
        {
            var product = await GetEntityByIDAsync(id);
            if (product == null)
                return false;

            //position
            var quan = (productSaveDTO.Quantity ?? 0) - (product.Quantity ?? 0);

            product.Name = productSaveDTO.Name;
            product.UnitPrice = productSaveDTO.UnitPrice;
            product.Unit = productSaveDTO.Unit;
            product.ProductCategoryId = productSaveDTO.ProductCategoryId;
            product.Quantity = productSaveDTO.Quantity;
            product.Ram = productSaveDTO.Ram;
            product.Cpu = productSaveDTO.Cpu;
            product.Screen = productSaveDTO.Screen;
            product.Pin = productSaveDTO.Pin;
            product.Origin = productSaveDTO.Origin;
            product.WarrantyTime = productSaveDTO.WarrantyTime;
            product.LunchTime = productSaveDTO.LunchTime;
            product.PositionId = productSaveDTO.PositionId;
            product.Image = productSaveDTO.Image;

            await UpdateEntityAsync(product);

            //Thêm số lượng cho bên position
            var position = context.Set<Position>().Find(product.PositionId);
            if (position != null)
            {
                position.Quantity = Math.Max((position.Quantity ?? 0) + (quan), 0);
                context.Set<Position>().Attach(position);
                context.Entry(position).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<int> DeleteProduct(string id)
        {
            var product = await GetEntityByIDAsync(id);
            if (product == null)
                return 0;

            //Thêm số lượng cho bên position
            var position = context.Set<Position>().Find(product.PositionId);
            if (position != null)
            {
                position.Quantity = Math.Max((position.Quantity ?? 0) - (product.Quantity ?? 0), 0);
                context.Set<Position>().Attach(position);
                context.Entry(position).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            product.IsDeleted = true;

            return await UpdateEntityAsync(product);
        }

        public async Task<PagingResponse> GetProductPaging(PagingRequest paging)
        {
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //f => true có thể sửa theo nghiệp vụ ví dụ như f.Status = true;
            var result = await FilterEntitiesPagingAsync(f => f.IsDeleted != true, paging.Search, paging.SearchField, paging.Sort, paging.Page, paging.PageSize);
            var categories = await context.Set<ProductCategory>().AsNoTracking().ToListAsync();
            var positions = await context.Set<Position>().AsNoTracking().ToListAsync();

            var products = result.Data.Select(f =>
            {
                var viewDTO = new ProductViewDTO();
                viewDTO = Mapper.MapInit<Product, ProductViewDTO>(f);
                viewDTO.ProductCategoryName = categories.FirstOrDefault(c => c.Id == f.ProductCategoryId)?.Name ?? string.Empty;
                viewDTO.PositionName = positions.FirstOrDefault(c => c.Id == f.PositionId)?.Name ?? string.Empty;
                return viewDTO;
            }).ToList();    

            pagingResponse.Data = products ?? new List<ProductViewDTO>();
            pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
