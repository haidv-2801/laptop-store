using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core.Anotation;

namespace LaptopStore.Data.ModelDTO.Customer
{
    public class CustomerSaveDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        [DisplayName("Tên")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Họ không được để trống")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        [DisplayName("Họ")]
        public string? LastName { get; set; }
        [RegularExpression(@"^\+?0\d{9,10}$",ErrorMessage = "Số điện thoại không đúng định dạng")]
        [DisplayName("Số điện thoại")]
        public string? Phone { get; set; }
        //[EmailFormat(ErrorMessage = "Email không đúng định dạng")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
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
