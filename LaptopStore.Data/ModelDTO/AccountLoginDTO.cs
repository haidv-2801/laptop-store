using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Data.ModelDTO
{
    public class AccountLoginDTO
    {
        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
    }
}
