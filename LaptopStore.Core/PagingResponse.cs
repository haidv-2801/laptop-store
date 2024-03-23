using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core
{
    public class PagingResponse
    {
        public object Data { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    //public class PagingResponse<TEntity>
    //{
    //    public IEnumerable<TEntity> Entities { get; set; }
    //    public int Total { get; set; }
    //    public int Page { get; set; }
    //    public int PageSize { get; set; }

    //    public int TotalPage { get; set; }
    //}
}
