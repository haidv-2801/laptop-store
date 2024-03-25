using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace LaptopStore.Data.Models
{
    public partial class Supplier
    {
        public string Id { get; set; } = null!;
        [StringLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự")]
        [DisplayName("Tên nhà cung cấp")]
        public string? Name { get; set; }
        public string? ContactName { get; set; }

        [DisplayName("Số điện thoại")]
        public string? Phone { get; set; }
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
