using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.ProductCategory;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.ProductCategoryService
{
    public interface IProductCategoryService : IBaseService<ProductCategory>
    {
        Task<List<ProductCategory>> GetAll();
        Task<ProductCategory> GetById(string id);

        Task<int> SaveProductCategory(ProductCategorySaveDTO productCategorySaveDTO);

        Task<PagingResponse> GetProductCategoryPaging(PagingRequest paging);

        Task<bool> UpdateProductCategory(string id, ProductCategorySaveDTO productCategorySaveDTO);

        Task<int> DeleteProductCategory(string id);
        Task<bool> CheckDuplicateName(string name);
        Task<bool> CheckDuplicateNameNotThis(string id, string name);
        Task<bool> CheckExistsProduct(string id);
    }
}
