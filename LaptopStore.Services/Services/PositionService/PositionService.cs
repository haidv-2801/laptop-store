using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using System.Net.Http.Json;
using Newtonsoft.Json;
using LaptopStore.Core.Enums;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.PositionService
{
    public class PositionService : BaseService<Position>, IPositionService
    {
        public PositionService(ApplicationDbContext dbContext):base(dbContext)
        {
        }

        public async Task<List<Position>> GetAll()
        {
            return await dbSet.OrderByDescending(e => e.Name).ToListAsync();
        }

        public async Task<Position> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }
        public async Task<int> SavePosition(PositionSaveDTO positionSaveDTO)
        {
            var position = Mapper.MapInit<PositionSaveDTO, Position>(positionSaveDTO);
            await AddEntityAsync(position);
            return 1;
        }

        public async Task<bool> UpdatePosition(string id, PositionSaveDTO positionSaveDTO)
        {
            var position = await GetEntityByIDAsync(id);
            if (position == null)
                return false;
            position.Name = positionSaveDTO.Name;
            await UpdateEntityAsync(position);
            return true;
        }

        public async Task<int> DeletePosition(string id)
        {
            var position = await GetEntityByIDAsync(id);
            if (position == null)
                return 0;

            return await DeleteEntityAsync(position);
        }

        public async Task<bool> CheckDuplicateName(string name)
        {
            var position = context.Set<Position>().AsNoTracking().FirstOrDefault(e => e.Name == name);
            if (position == null)
                return false;

            return true;
        }

        public async Task<bool> CheckExistsProduct(string id)
        {
            var product = context.Set<Product>().AsNoTracking().FirstOrDefault(e => e.PositionId == id);
            if (product == null)
                return false;

            return true;
        }

        public async Task<PagingResponse> GetPositionPaging(PagingRequest paging)
        {
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //f => true có thể sửa theo nghiệp vụ ví dụ như f.Status = true;
            var result = await FilterEntitiesPagingAsync(f => true, paging.Search, paging.SearchField, paging.Sort, paging.Page, paging.PageSize);
                pagingResponse.Data = result.Data;
                pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
