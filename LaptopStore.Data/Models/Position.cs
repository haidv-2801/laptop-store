using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class Position
    {
        public Position()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Acreage { get; set; }
        public int? Quantity { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
