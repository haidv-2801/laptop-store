using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public partial class WarehouseExport
    {
        public WarehouseExport()
        {
            WarehouseExportDetails = new HashSet<WarehouseExportDetail>();
        }

        public string Id { get; set; } = null!;
        public DateTime ExportTime { get; set; }
        [DisplayName("Trạng thái")]
        public int Status { get; set; }
        [DisplayName("Nhân viên")]
        public string Username { get; set; } = null!;
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
