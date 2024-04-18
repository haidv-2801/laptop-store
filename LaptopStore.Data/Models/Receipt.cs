using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class Receipt : BaseEntity
    {
        public Receipt()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
        }

        public string Id { get; set; } = null!;
        [DisplayName("Thời gian nhập")]
        public DateTime ImportTime { get; set; }
        [DisplayName("Trạng thái")]
        public int Status { get; set; }

        [DisplayName("Người nhập")]
        public string Username { get; set; } = null!;
        [DisplayName("Nhà cung cấp")]
        public string SupplierId { get; set; }
        [DisplayName("Mã đơn")]
        public string? Code { get; set; }

        public virtual Supplier Supplier { get; set; } = null!;
        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
