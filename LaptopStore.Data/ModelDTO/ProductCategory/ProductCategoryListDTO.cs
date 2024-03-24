using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.ModelDTO.ProductCategory
{
    public class ProductCategoryListDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DisplayName("Tên danh mục")]
        public string Name { get; set; }
        [DisplayName("Số lượng sản phẩm")]
        public int ProductQuantity { get; set; }

    }
}
