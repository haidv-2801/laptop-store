using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core.Anotation;
namespace LaptopStore.Data.Models
{
    public partial class Supplier
    {
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Tên NCC không được để trống")]
        [StringLength(255, ErrorMessage = "Tên NCC không được vượt quá 255 ký tự")]
        [DisplayName("Tên nhà cung cấp")]
        public string? Name { get; set; }
        [DisplayName("Tên liên hệ")]
        public string? ContactName { get; set; }
        [PhoneNumberFormat(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [DisplayName("Số điện thoại")]
        public string? Phone { get; set; }
        [EmailFormat(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
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
