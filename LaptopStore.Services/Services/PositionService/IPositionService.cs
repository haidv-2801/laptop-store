using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.PositionService
{
    public interface IPositionService : IBaseService<Position>
    {
        Task<List<Position>> GetAll();
        Task<Position> GetById(string id);

        Task<int> SavePosition(PositionSaveDTO positionSaveDTO);

        Task<PagingResponse> GetPositionPaging(PagingRequest paging);

        Task<bool> UpdatePosition(string id, PositionSaveDTO positionSaveDTO);

        Task<int> DeletePosition(string id);

        Task<bool> CheckDuplicateName(string name);
        Task<bool> CheckDuplicateNameNotThis(string id, string name);
        Task<bool> CheckExistsProduct(string id);
    }
}
