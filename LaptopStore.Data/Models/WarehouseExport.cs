using System;
using System.Collections.Generic;

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
        public int Status { get; set; }
        public string Username { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
