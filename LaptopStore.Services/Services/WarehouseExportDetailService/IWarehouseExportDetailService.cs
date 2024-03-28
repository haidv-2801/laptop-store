/*using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;
using LaptopStore.Data.ModelDTO.WarehouseExportDetail;

namespace LaptopStore.Services.Services.WarehouseExportService
{
    public interface IWarehouseExportService : IBaseService<WarehouseExportDetail>
    {
        Task<List<WarehouseExportDetail>> GetAll();
        Task<WarehouseExportDetail> GetById(string id);

        Task<int> SaveWarehouseExport(WarehouseExportSaveDTO saveDTO);

        Task<PagingResponse> GetWarehouseExportPaging(PagingRequest paging);

        Task<bool> UpdateWarehouseExport(string id, WarehouseExportDetail saveDTO);

        Task<int> DeleteWarehouseExport(string id);
    }
}
*/