using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.SupplierService
{
    public interface ISupplierService : IBaseService<Supplier>
    {
        Task<List<Supplier>> GetAll();
        Task<Supplier> GetById(string id);

        Task<int> SaveSupplier(SupplierSaveDTO supplierSaveDTO);

        Task<PagingResponse> GetSupplierPaging(PagingRequest paging);

        Task<bool> UpdateSupplier(string id, SupplierSaveDTO supplierSaveDTO);

        Task<int> DeleteSupplier(string id);

        Task<bool> CheckDuplicateName(string name);
        Task<bool> CheckDuplicateNameNotThis(string id, string name);
    }
}
