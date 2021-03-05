using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TplDemo.Common.Helper
{
    public class JsonData
    {
        /// <summary>把实体序列化json对象</summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string Serializer(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }

        /// <summary>把josn对象转换成实体</summary>
        /// <param name="StrJson"></param>
        /// <returns></returns>
        public static object Deserialize(string StrJson)
        {
            return JsonConvert.DeserializeObject<object>(StrJson);
        }

        /// <summary>把josn对象转换成实体</summary>
        /// <param name="StrJson"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string StrJson)
        {
            return JsonConvert.DeserializeObject<T>(StrJson);
        }

        /// <summary>Xml格式字符转换为T类型的对象</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ParseFormByXml<T>(string xml, string rootName = "root")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
            StringReader reader = new StringReader(xml);

            T res = (T)serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            return res;
        }
    }
}