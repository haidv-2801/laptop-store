using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaptopStore.Data.ModelDTO
{
    public class PositionSaveDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Tên vị trí không được để trống")]
        [StringLength(50, ErrorMessage = "Tên vị trí không được vượt quá 50 ký tự")]
        [DisplayName("Tên vị trí")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Diện tích không được để trống")]
        [DisplayName("Diện tích")]
        public double Acreage { get; set; }

        [DisplayName("Số lượng sản phẩm")]
        public int? Quantity { get; set; }

    }
}
