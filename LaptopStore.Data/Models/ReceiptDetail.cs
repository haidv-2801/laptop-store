using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class ReceiptDetail
    {
        public string Id { get; set; } = null!;
        public string ReceiptId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Receipt Receipt { get; set; } = null!;
    }
}
