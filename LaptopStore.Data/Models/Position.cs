using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.Models
{
    public partial class Position
    {
        public Position()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Tên vị trí không được để trống")]
        [StringLength(50, ErrorMessage = "Tên vị trí không được vượt quá 50 ký tự")]
        [DisplayName("Tên vị trí")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Diện tích không được để trống")]
        [DisplayName("Diện tích")]
        public double Acreage { get; set; }
        [DisplayName("Số lượng sản phẩm")]
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
