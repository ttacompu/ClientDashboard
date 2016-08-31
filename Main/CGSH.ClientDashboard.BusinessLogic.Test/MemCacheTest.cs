using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.BusinessLogic.Util;

namespace CGSH.ClientDashboard.BusinessLogic.Test
{
    [TestClass]
    public class MemCacheTest
    {
        IMemCache _MemCache;

        [TestInitialize]
        public void SetupMethod()
        {
            _MemCache = new MemCache();
        }


        [TestMethod]
        [TestCategory("MemCache")]
        public void MemCache_Set_Get_Type()
        {
            Client client = new Client() { Name = "client" };
            _MemCache.Set("test", client);

            Client fromCache;
            var result=_MemCache.TryGet("test", out fromCache);

            Assert.AreEqual(true, result);
            Assert.AreEqual("client", fromCache.Name);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _MemCache = null;
        }


    }
}
