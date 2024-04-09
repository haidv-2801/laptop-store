using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class WarehouseExport : BaseEntity
    {
        public WarehouseExport()
        {
            WarehouseExportDetails = new HashSet<WarehouseExportDetail>();
        }

        public string Id { get; set; } = null!;
        [DisplayName("Thời gian xuất")]
        public DateTime ExportTime { get; set; }
        [DisplayName("Trạng thái")]
        public int Status { get; set; }
        [DisplayName("Nhân viên")]
        public string Username { get; set; }
        [DisplayName("Khách hàng")]
        public string CustomerId { get; set; } = null!;
        [DisplayName("Mã đơn")]
        public string? Code { get; set; }

        [DisplayName("Khách hàng")]
        public virtual Customer Customer { get; set; } = null!;
        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
