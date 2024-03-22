using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core
{
    public class PagingRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// FullName:Desc
        /// </summary>
        public string Sort { get; set; }
        public string Search { get; set; }
    }
}
