using System;
using System.Runtime.Caching;

namespace CGSH.Clientdashboard.Interface.Util
{
    public class ICache
    {
        ObjectCache cache = MemoryCache.Default;

    }
}
