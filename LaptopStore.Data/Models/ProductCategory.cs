using LaptopStore.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public partial class ProductCategory : BaseEntity
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; } = null!;
        [DisplayName("Tên danh mục")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
