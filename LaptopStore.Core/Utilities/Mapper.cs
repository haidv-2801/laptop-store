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
        /// <summary>
        /// Mapping tạo instance mới
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <typeparam name="TDes"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDes MapInit<TSrc, TDes>(TSrc source)
        {
            TinyMapper.Bind<TSrc, TDes>();
            return (TDes)TinyMapper.Map(typeof(TSrc), typeof(TDes), source);
        }

        /// <summary>
        /// Mapping vào target nhưng không tạo instance mới
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <typeparam name="TDes"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void MapUpdate<TSrc, TDes>(TSrc source, TDes target)
        {
            TinyMapper.Bind<TSrc, TDes>();
            TinyMapper.Map(source, target);
        }
    }
}
