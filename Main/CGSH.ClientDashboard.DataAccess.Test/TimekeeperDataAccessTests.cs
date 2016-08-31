using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGSH.ClientDashboard.BusinessEntitity;


namespace CGSH.ClientDashboard.DataAccess.Tests
{
    [TestClass()]
    public class TimekeeperDataAccessTests
    {
        [TestMethod()]
        public async Task GetPartnersAndSeniorCounselTest()
        {
            TimekeeperDataAccess t = new TimekeeperDataAccess(null);
            List<Person> people = await t.GetPartnersAndSeniorCounsel("04021", DateTime.Now.AddMonths(-12), DateTime.Now, 1);
            Assert.IsNotNull(people);
        }
    }
}