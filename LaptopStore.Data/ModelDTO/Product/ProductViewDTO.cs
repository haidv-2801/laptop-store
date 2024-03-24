using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public class ProductViewDTO
    {
        public ProductViewDTO()
        {
        }

        public string Id { get; set; } = null!;

        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; } = null!;

        [DisplayName("Đơn giá")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Đơn vị tính")]
        public string Unit { get; set; } = null!;

        [DisplayName("Thể loại")]
        public string ProductCategoryId { get; set; } = null!;

        public string ProductCategoryName { get; set; } = null!;

        [DisplayName("Số lượng")]
        public int? Quantity { get; set; }

        [DisplayName("Ram")]
        public int? Ram { get; set; }
        public string? Cpu { get; set; }

        [DisplayName("Màn hình")]
        public string? Screen { get; set; }
        public string? Pin { get; set; }
        public string? Origin { get; set; }

        [DisplayName("Bảo hành")]
        public string? WarrantyTime { get; set; }
        public DateTime? LunchTime { get; set; }

        [DisplayName("Vị trí")]
        public string PositionId { get; set; } = null!;
        public string PositionName { get; set; } = null!;

        [DisplayName("Ảnh")]
        public string? Image { get; set; }
    }
}
