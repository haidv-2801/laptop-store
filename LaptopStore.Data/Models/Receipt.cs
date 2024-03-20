using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class Receipt
    {
        public Receipt()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
        }

        public string Id { get; set; } = null!;
        public DateTime ImportTime { get; set; }
        public int Status { get; set; }
        public string Username { get; set; } = null!;

        public virtual Account UsernameNavigation { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
