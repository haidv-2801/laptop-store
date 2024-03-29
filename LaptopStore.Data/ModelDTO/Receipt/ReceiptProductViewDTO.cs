using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core.Anotation;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.WarehouseExport;

namespace LaptopStore.Data.ModelDTO.Receipt
{
    public class ReceiptProductViewDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Image { get; set; }

        public int? Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total 
        { 
            get 
            {
                return (Quantity ?? 0) * UnitPrice;
            }
        }
    }
}
