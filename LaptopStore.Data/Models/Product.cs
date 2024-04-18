using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
            WarehouseExportDetails = new HashSet<WarehouseExportDetail>();
        }

        public string Id { get; set; } = null!;

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; } = null!;

        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Đơn giá không được để trống")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Đơn vị tính")]
        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public string Unit { get; set; } = null!;

        [Display(Name = "Loại sản phẩm")]
        [Required(ErrorMessage = "Loại sản phẩm không được để trống")]
        public string ProductCategoryId { get; set; } = null!;

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int? Quantity { get; set; }
        
        [Required(ErrorMessage = "Ram không được để trống")]
        public int? Ram { get; set; }

        [Required(ErrorMessage = "Cpu không được để trống")]
        public string? Cpu { get; set; }

        [Display(Name = "Màn hình")]
        [Required(ErrorMessage = "Màn hình không được để trống")]
        public string? Screen { get; set; }

        [Required(ErrorMessage = "Pin không được để trống")]
        public string? Pin { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public string? Origin { get; set; }

        [Display(Name = "Thời gian bảo hành")]
        [Required(ErrorMessage = "Thời gian bảo hành không được để trống")]
        public string? WarrantyTime { get; set; }

        [Display(Name = "Thời gian kích hoạt")]
        [Required(ErrorMessage = "Thời gian kích hoạt không được để trống")]
        public DateTime? LunchTime { get; set; }

        [Display(Name = "Vị trí kho")]
        [Required(ErrorMessage = "Vị trí kho không được để trống")]
        public string PositionId { get; set; } = null!;

        [Display(Name = "Ảnh")]
        public string? Image { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public virtual Position Position { get; set; } = null!;
        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual ICollection<WarehouseExportDetail> WarehouseExportDetails { get; set; }
    }
}
