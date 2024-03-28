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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DisplayName("Thời gian xuất")]
        public DateTime ImportTime { get; set; }
        [DisplayName("Trạng thái")]
        public int Status { get; set; }
        [DisplayName("Nhân viên")]
        public string Username { get; set; } = null!;
        [DisplayName("Khách hàng")]
        public string CustomerId { get; set; } = null!;
        public List<ProductExportDTO> Products { get; set; } = null!;
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; }

    }
}
