using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core
{
    public class BaseEntity
    {
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; } = null;
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; } = null;
    }
}
