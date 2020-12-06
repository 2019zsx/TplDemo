using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common
{
    public class HttpRuntimeCache<V> : ICacheService
    {
        #region 全局变量

        private static HttpRuntimeCache<V> _instance = null;
        private static readonly object _instanceLock = new object();

        #endregion 全局变量

        #region 构造函数

        private HttpRuntimeCache()
        {
        }

        #endregion 构造函数

        public static HttpRuntimeCache<V> GetInstance()
        {
            if (_instance == null)
                lock (_instanceLock)
                    if (_instance == null)
                        _instance = new HttpRuntimeCache<V>();
            return _instance;
        }

        public void Add<V>(string key, V value)
        {
            HttpRuntimeCache<V>.GetInstance().Add(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            HttpRuntimeCache<V>.GetInstance().Add(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return HttpRuntimeCache<V>.GetInstance().ContainsKey<V>(key);
        }

        public V Get<V>(string key)
        {
            return HttpRuntimeCache<V>.GetInstance().Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return HttpRuntimeCache<V>.GetInstance().GetAllKey<V>();
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            var cacheManager = HttpRuntimeCache<V>.GetInstance();
            if (cacheManager.ContainsKey<V>(cacheKey))
            {
                return cacheManager.Get<V>(cacheKey);
            }
            else
            {
                var result = create();
                cacheManager.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            HttpRuntimeCache<V>.GetInstance().Remove<V>(key);
        }
    }
}