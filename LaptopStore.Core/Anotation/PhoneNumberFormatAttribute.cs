using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Core.Anotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PhoneNumberFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true; // Cho phép giá trị null hoặc rỗng

            // Regex cho định dạng số điện thoại Việt Nam: 10 hoặc 11 số, bắt đầu bằng '0', có thể có dấu '+' ở đầu
            string phoneNumberPattern = @"^\+?0\d{9,10}$";

            if (System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), phoneNumberPattern))
                return true; // Số điện thoại hợp lệ
            else
                return false; // Số điện thoại không hợp lệ
        }
    }
}
