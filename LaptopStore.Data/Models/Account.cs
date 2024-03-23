using LaptopStore.Core;
using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class Account
    {
        public Account()
        {
            Receipts = new HashSet<Receipt>();
            WarehouseExports = new HashSet<WarehouseExport>();
        }

        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public int AccountType { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<WarehouseExport> WarehouseExports { get; set; }
    }
}
