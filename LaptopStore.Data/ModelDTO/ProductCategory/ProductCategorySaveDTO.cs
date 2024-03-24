using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.ModelDTO.ProductCategory
{
    public class ProductCategorySaveDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; }

    }
}
