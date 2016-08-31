using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGSH.ClientDashboard.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGSH.ClientDashboard.BusinessEntitity;

namespace CGSH.ClientDashboard.DataAccess.Tests
{
    [TestClass()]
    public class SearchDataAccessTests
    {
        [TestMethod()]
        public async Task GetTest()
        {
            SearchDataAccess s = new SearchDataAccess(null);
            List<ClientGroup> clientGroups = await s.Get("Citi");
            Assert.IsNotNull(clientGroups);
        }
    }
}