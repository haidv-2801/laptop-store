using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.ProductService
{
    public interface IProductService : IBaseService<Product>
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(string id);

        Task<int> SaveProduct(ProductSaveDTO saveDTO);

        Task<PagingResponse> GetProductPaging(PagingRequest paging);

        Task<bool> UpdateProduct(string id, ProductSaveDTO saveDTO);

        Task<int> DeleteProduct(string id);
    }
}
