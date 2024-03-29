using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;
using LaptopStore.Data.ModelDTO.WarehouseExport;

namespace LaptopStore.Services.Services.WarehouseExportService
{
    public interface IWarehouseExportService : IBaseService<WarehouseExport>
    {
        Task<List<WarehouseExport>> GetAll();
        Task<WarehouseExport> GetById(string id);

        Task<WarehouseExport> GetByIDIncludesDetail(string id);

        Task<int> SaveWarehouseExport(WarehouseExportSaveDTO saveDTO);

        Task<PagingResponse> GetWarehouseExportPaging(PagingRequest paging);

        Task<bool> UpdateWarehouseExport(string id, WarehouseExportSaveDTO saveDTO);

        Task<int> DeleteWarehouseExport(string id);
    }
}
