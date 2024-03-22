using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core.Enums
{
    public enum ResponseCode
    {
        Success = 200,
        BusinessError = 400,
        ServerError = 500,
        PermissionError = 401
    }
}
