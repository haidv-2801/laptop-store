using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core.Anotation;
using LaptopStore.Data.Models;

namespace LaptopStore.Data.ModelDTO.WarehouseExport
{
    public class ProductExportDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DisplayName("Thời gian xuất")]
        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

    }
}
