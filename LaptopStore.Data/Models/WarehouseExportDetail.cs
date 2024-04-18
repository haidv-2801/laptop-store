using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class WarehouseExportDetail : BaseEntity
    {
        public string Id { get; set; } = null!;
        public string WarehouseExportId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual WarehouseExport WarehouseExport { get; set; } = null!;
    }
}
