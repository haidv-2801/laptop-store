using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Core.Anotation
{
    public class EmailFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true; // Cho phép giá trị null (có thể kiểm tra bắt buộc bằng RequiredAttribute)
            }

            string email = value as string;
            if (email == null)
            {
                return false; // Giá trị không phải là chuỗi
            }

            // Kiểm tra định dạng email bằng regex
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
    }
}
