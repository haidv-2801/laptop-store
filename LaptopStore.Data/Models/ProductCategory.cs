using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
