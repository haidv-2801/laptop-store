using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;
using LaptopStore.Data.ModelDTO.Receipt;

namespace LaptopStore.Services.Services.ProductService
{
    public interface IReceiptService : IBaseService<Receipt>
    {
        Task<List<Receipt>> GetAll();
        Task<Receipt> GetById(string id);

        Task<int> SaveReceipt(ReceiptSaveDTO saveDTO);

        Task<PagingResponse> GetReceiptPaging(PagingRequest paging);

        Task<bool> UpdateReceipt(string id, Receipt saveDTO);

        Task<int> DeleteReceipt(string id);
    }
}
