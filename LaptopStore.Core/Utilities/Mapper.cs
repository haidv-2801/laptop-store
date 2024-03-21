using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Core.Utilities
{
    public static class Mapper
    {
        public static TDes? MapInit<TSrc, TDes>(TSrc source)
        {
            TinyMapper.Bind<TSrc, TDes>();
            return TinyMapper.Map(source, default(TDes));
        }
    }
}
