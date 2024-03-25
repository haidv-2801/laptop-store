﻿using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class Product
    {
        public Product()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
            WarehouseExportDetails = new HashSet<WarehouseExportDetail>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public string Unit { get; set; } = null!;
        public string ProductCategoryId { get; set; } = null!;
        public int? Quantity { get; set; }
        public int? Ram { get; set; }
        public string? Cpu { get; set; }
        public string? Screen { get; set; }
        public string? Pin { get; set; }
        public string? Origin { get; set; }
        public string? WarrantyTime { get; set; }
        public DateTime? LunchTime { get; set; }
        public string PositionId { get; set; } = null!;
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Position Position { get; set; } = null!;
        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
