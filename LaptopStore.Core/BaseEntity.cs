using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core
{
    public class BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null;

        public DateTime? ModifiedAt { get; set; }
        
        public string ModifiedBy { get; set; } = null;
    }
}
