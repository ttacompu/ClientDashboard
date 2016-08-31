using CGSH.ClientDashboard.Interface.BusinessLogic;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessLogic.Util
{
    public class MemCache : IMemCache
    {
        ObjectCache cacheObj = MemoryCache.Default;
        private int DefaultMinutes = 15;

        [InjectionConstructor]
        public MemCache() {
            try
            {
                var cacheTime = System.Configuration.ConfigurationManager.AppSettings["DefaultCacheTime"];
                if (!string.IsNullOrEmpty(cacheTime)) {
                    DefaultMinutes = int.Parse(cacheTime);
                }
            }
            catch
            {
                ;
            }
            
        }

        public MemCache(string name, int min) {
            cacheObj = new MemoryCache(name);
            DefaultMinutes = min;

        }
        public void Set<T>(string cacheKey, T cacheItem)
        {
            cacheObj.Set(cacheKey, cacheItem, GetCacheItemPolicy(DefaultMinutes));
        }
        public void Set<T>(string cacheKey, T cacheItem, int minutes) {
            cacheObj.Set(cacheKey, cacheItem, GetCacheItemPolicy(minutes));
        }

        public bool TryGet<T>(string cacheKey, out T returnItem) {
            Type t = typeof(T);
            var tmp=cacheObj[cacheKey];

            if (tmp != null) {
                returnItem = (T)tmp;
                return true;
            }

            returnItem = default(T);
            return false;
        }

        public bool TryGetAndSet<T>(string cacheKey, Func<T> getData, out T returnData) {
            Type t = typeof(T);
            bool retrievedFromCache = TryGet<T>(cacheKey, out returnData);

            if (retrievedFromCache)
            {
                return true;
            }
            else {
                returnData = getData();
                Set<T>(cacheKey, returnData);
                return returnData != null;
            }

        }

        public  bool TryGetAndSet<T>(string cacheKey, Func<T> getData, int minutes, out T returnData)
        {
            Type t = typeof(T);
            bool retrievedFromCache = TryGet<T>(cacheKey, out returnData);
            if (retrievedFromCache)
            {
                return true;
            }
            else
            {
                returnData = getData();
                Set<T>(cacheKey, returnData, minutes);
                return returnData != null;
            }
        }

        public void Remove(string cacheKey) {
            cacheObj.Remove(cacheKey);
        }

        public void RemoveAll()
        {
            foreach (var element in cacheObj)
            {
                cacheObj.Remove(element.Key);
            }

        }
        private CacheItemPolicy GetCacheItemPolicy(int minutes = 15)
        {
            var policy = new CacheItemPolicy()
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes)
            };
            return policy;
        }

        
    }
}
