using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LaptopStore.Core;

namespace LaptopStore.Data.Models
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
            Receipts = new HashSet<Receipt>();
            WarehouseExports = new HashSet<WarehouseExport>();
        }
        
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Tài khoản không được vượt quá 50 ký tự")]
        [DisplayName("Tài khoản")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; } = null!;
        [DisplayName("Giới tính")]
        public int? Gender { get; set; }
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Vai trò không được để trống")]
        [DisplayName("Vai trò")]
        public int AccountType { get; set; }
        [DisplayName("Trạng thái")]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<WarehouseExport> WarehouseExports { get; set; }
    }
}
