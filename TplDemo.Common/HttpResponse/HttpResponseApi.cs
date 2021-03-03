using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Common.Config;
using TplDemo.Common.IsWhatExtenions;

namespace TplDemo.Common.HttpResponse
{
    public class HttpResponseApi<T> where T : class, new()
    {
        public T Get(apiurl requesturl, string url, object pars, string Token = "", bool _bool = false)
        {
            var type = RestSharp.Method.GET;

            IRestResponse<T> reval = GetApiInfo(requesturl, url, pars, type, Token);
            return reval.Data;
        }

        public T Post(apiurl requesturl, string url, object pars, string Token = "", bool _bool = false)
        {
            var type = RestSharp.Method.POST;
            IRestResponse<T> reval = GetApiInfo(requesturl, url, pars, type, Token);
            return reval.Data;
        }

        public T Delete(apiurl requesturl, string url, object pars, string Token = "")
        {
            var type = RestSharp.Method.DELETE;
            IRestResponse<T> reval = GetApiInfo(requesturl, url, pars, type, Token);
            return reval.Data;
        }

        public T Put(apiurl requesturl, string url, object pars, string Token = "")
        {
            var type = RestSharp.Method.PUT;
            IRestResponse<T> reval = GetApiInfo(requesturl, url, pars, type, Token);
            return reval.Data;
        }

        private static IRestResponse<T> GetApiInfo(apiurl requesturl, string url, object pars, RestSharp.Method type, string Token = "", string ContentType = "")
        {
            var request = new RestRequest(type);
            if (pars != null)
                request.AddObject(pars);
            if (!ContentType.IsEmpty())
            {
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
            }
            string baseutl = BaseConfigModel.Configuration[$"weburl:{requesturl.ToString()}"] + url;
            var client = new RestClient(baseutl);
            /*
             判断Token是否为，是否需要添加验证
             */
            if (!Token.IsEmpty())
            {
                request.AddHeader("Authorization", Token);
            }
            client.CookieContainer = new System.Net.CookieContainer();
            IRestResponse<T> reval = client.Execute<T>(request);
            if (reval.ErrorException != null)
            {
                // PubMethod.WirteExp(new Exception(reval.Content));
                throw new Exception("请求出错");
            }
            return reval;
        }
    }
}