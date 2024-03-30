using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core.Anotation;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.WarehouseExport;

namespace LaptopStore.Data.ModelDTO.Receipt
{
    public class ReceiptSaveDTO
    {
        [DisplayName("Thời gian xuất")]
        public DateTime ImportTime { get; set; }

        [DisplayName("Trạng thái")]
        public int Status { get; set; }

        [DisplayName("Nhân viên")]
        public string Username { get; set; } = null!;

        public string AccountId { get; set; } = null!;

        public string SupplierId { get; set; } = null!;

        public List<ProductExportDTO> Products { get; set; } = null!;
    }
}
