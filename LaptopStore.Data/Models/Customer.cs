using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public partial class Customer
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
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Người tạo")]
        public string? CreatedBy { get; set; }
        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Người sửa")]
        public string? ModifiedBy { get; set; }
    }
}
