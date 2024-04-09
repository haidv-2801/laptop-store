using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class Customer: BaseEntity
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Điện thoại")]
        public string? Phone { get; set; }
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }
    }
}
