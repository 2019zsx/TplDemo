using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Helper
{
    public class JsonData
    {
        /// <summary>
        /// 把实体序列化json对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string Serializer(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }

        /// <summary>
        /// 把josn对象转换成实体
        /// </summary>
        /// <param name="StrJson"></param>
        /// <returns></returns>
        public static object Deserialize(string StrJson)
        {
            return JsonConvert.DeserializeObject<object>(StrJson);
        }

        /// <summary>
        /// 把josn对象转换成实体
        /// </summary>
        /// <param name="StrJson"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string StrJson)
        {
            return JsonConvert.DeserializeObject<T>(StrJson);
        }
    }
}