using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = null!;

        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; } = null!;

        [Display(Name = "Loại sản phẩm")]
        public string ProductCategoryId { get; set; } = null!;

        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }

        public int? Ram { get; set; }
        public string? Cpu { get; set; }

        [Display(Name = "Màn hình")]
        public string? Screen { get; set; }
        public string? Pin { get; set; }
        public string? Origin { get; set; }

        [Display(Name = "Thời gian bảo hành")]
        public string? WarrantyTime { get; set; }

        [Display(Name = "Thời gian kích hoạt")]
        public DateTime? LunchTime { get; set; }

        [Display(Name = "Vị trí kho")]
        public string PositionId { get; set; } = null!;

        [Display(Name = "Ảnh")]
        public string? Image { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }

        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public virtual Position Position { get; set; } = null!;
        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
