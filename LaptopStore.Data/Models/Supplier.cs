using System;
using System.Collections.Generic;

namespace LaptopStore.Data.Models
{
    public partial class Supplier
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
