using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.ProductCategory;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.ProductCategoryService
{
    public class ProductCategoryService : BaseService<ProductCategory>, IProductCategoryService
    {
        public ProductCategoryService(ApplicationDbContext dbContext):base(dbContext)
        {
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await dbSet.OrderByDescending(e => e.Name).ToListAsync();
        }

        public async Task<ProductCategory> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }
        public async Task<int> SaveProductCategory(ProductCategorySaveDTO productCategorySaveDTO)
        {
            var productCategory = Mapper.MapInit<ProductCategorySaveDTO, ProductCategory>(productCategorySaveDTO);
            await AddEntityAsync(productCategory);
            return 1;
        }

        public async Task<bool> UpdateProductCategory(string id, ProductCategorySaveDTO productCategorySaveDTO)
        {
            var productCategory = await GetEntityByIDAsync(id);
            if (productCategory == null)
                return false;
            productCategory.Name = productCategorySaveDTO.Name;
            await UpdateEntityAsync(productCategory);
            return true;
        }

        public async Task<int> DeleteProductCategory(string id)
        {
            var productCategory = await GetEntityByIDAsync(id);
            if (productCategory == null)
                return 0;

            return await DeleteEntityAsync(productCategory);
        }
        public async Task<bool> CheckDuplicateName(string name)
        {
            bool? productCategory = null;
            if (productCategory == null)
                return false;

            return true;
        }
        
        public async Task<bool> CheckExistsProduct(string id)
        {
            var product = context.Set<Product>().AsNoTracking().FirstOrDefault(e => e.ProductCategoryId == id);
            if (product == null)
                return false;

            return true;
        }

        public async Task<PagingResponse> GetProductCategoryPaging(PagingRequest paging)
        {
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //f => true có thể sửa theo nghiệp vụ ví dụ như f.Status = true;
            var result = await FilterEntitiesPagingAsync(f => true, paging.Search, paging.SearchField, paging.Sort, paging.Page, paging.PageSize);
            

            var productCategories = result.Data.Select(async f =>
            {
                List<Product> productList= new List<Product>();
                var products = context.Set<Product>().AsNoTracking().Where(e=>e.ProductCategoryId == f.Id);
                if (products.Count()>0)
                {
                    productList = products.ToList();
                }
                var viewDTO = new ProductCategoryListDTO();
                viewDTO = Mapper.MapInit<ProductCategory, ProductCategoryListDTO>(f);
                viewDTO.ProductQuantity = productList.Count;
                return viewDTO;
            }).ToList();

            var newProductCategories = (await Task.WhenAll(productCategories)).ToList();

            pagingResponse.Data = newProductCategories ?? new List<ProductCategoryListDTO>();
            pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
