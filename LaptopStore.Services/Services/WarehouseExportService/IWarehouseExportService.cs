using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.WarehouseExportService
{
    public interface IWarehouseExportService : IBaseService<WarehouseExport>
    {
        Task<List<WarehouseExport>> GetAll();
        Task<WarehouseExport> GetById(string id);

        Task<int> SaveWarehouseExport(WarehouseExport saveDTO);

        Task<PagingResponse> GetWarehouseExportPaging(PagingRequest paging);

        Task<bool> UpdateWarehouseExport(string id, WarehouseExport saveDTO);

        Task<int> DeleteWarehouseExport(string id);
    }
}
