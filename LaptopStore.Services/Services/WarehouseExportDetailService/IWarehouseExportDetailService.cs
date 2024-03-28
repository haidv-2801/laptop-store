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

namespace LaptopStore.Services.Services.WarehouseExportDetailService
{
    public interface IWarehouseExportDetailService : IBaseService<WarehouseExportDetail>
    {
        Task<List<WarehouseExportDetail>> GetAll();
        Task<WarehouseExportDetail> GetById(string id);

        Task<int> SaveWarehouseExportDetail(WarehouseExportDetailSaveDTO saveDTO);

        Task<PagingResponse> GetWarehouseExportDetailPaging(PagingRequest paging);

        Task<bool> UpdateWarehouseExportDetail(string id, WarehouseExportDetail saveDTO);

        Task<int> DeleteWarehouseExportDetail(string id);
    }
}
