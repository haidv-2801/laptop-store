using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core.Utilities
{
    public static class Number
    {
        public static string Format(string number)
        {
            bool parsed = double.TryParse(number, out double value);
            if(!parsed)
                return number;
            return value.ToString("#,##0.00");
        }
    }
}
