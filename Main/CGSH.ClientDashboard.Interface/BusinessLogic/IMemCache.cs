using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.Interface.BusinessLogic
{
    public interface IMemCache
    {
        void Set<T>(string cacheKey, T cacheItem);
        void Set<T>(string cacheKey, T cacheItem, int minutes);
        bool TryGet<T>(string cacheKey, out T returnItem);
        bool TryGetAndSet<T>(string cacheKey, Func<T> getData, out T returnData);
        void Remove(string cacheKey);
        void RemoveAll();
    }
}
