using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.ModelDTO
{
    public class AccountSaveDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Tài khoản không được vượt quá 50 ký tự")]
        [DisplayName("Tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [DisplayName("Giới tính")]
        public int? Gender { get; set; }

        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [DisplayName("Vai trò")]
        public int AccountType { get; set; }
    }
}
