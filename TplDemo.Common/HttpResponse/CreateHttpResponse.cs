using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TplDemo.Common.Config;

namespace TplDemo.Common.HttpResponse
{
    public class CreateHttpResponse
    {
        // <summary>
        /// 发送http post请求 </summary> <param name="url">地址</param> <param
        /// name="parameters">查询参数集合</param> <returns></returns>
        public static string CreatePostHttpResponse(apiurl _apiurl, string url, string parameters, string token = "")
        {
            HttpWebRequest request = WebRequest.Create(BaseConfigModel.Configuration[$"weburl:{ _apiurl.ToString()}"] + url) as HttpWebRequest;//创建请求对象
            request.Method = "POST";//请求方式
            request.ContentType = "application/json";//链接类型
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", token);//构造查询字符串
            }
            byte[] data = Encoding.UTF8.GetBytes(parameters.ToString());
            //写入请求流
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return GetResponseString((HttpWebResponse)request.GetResponse());
        }

        public static string CreateGetHttpResponse(apiurl apiurl, string url, IDictionary<string, object> parameters, string token = "")
        {
            StringBuilder buffer = new StringBuilder();

            bool first = true;
            foreach (string key in parameters.Keys)
            {
                if (!first)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    first = false;
                }
            }
            url = url + "?" + buffer.ToString();
            HttpWebRequest request = WebRequest.Create(BaseConfigModel.Configuration[$"weburl:{apiurl.ToString()}"] + url) as HttpWebRequest;//创建请求对象
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";//链接类型 　

            return GetResponseString((HttpWebResponse)request.GetResponse());
        }

        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}