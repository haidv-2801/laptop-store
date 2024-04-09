using LaptopStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.BaseService
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> AddEntityAsync(T entity);
        Task<int> UpdateEntityAsync(T entity);
        Task<int> DeleteEntityAsync(T entity);
        Task<T> GetEntityByIDAsync(object id);
        Task<IEnumerable<T>> GetEntitiesAsync();
        Task<(IEnumerable<T> Data, int TotalRecords)> FilterEntitiesPagingAsync(
        Expression<Func<T, bool>> filter,
        string searchTerm,
        string searchFields,
        string sort,
        int pageNumber = 1,
        int pageSize = 10);
    }
}
