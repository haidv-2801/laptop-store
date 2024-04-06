using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public partial class Receipt
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
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; }
        [DisplayName("Nhà cung cấp")]
        public string SupplierId { get; set; }
        [DisplayName("Mã đơn")]
        public string? Code { get; set; }

        public virtual Supplier Supplier { get; set; } = null!;
        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
