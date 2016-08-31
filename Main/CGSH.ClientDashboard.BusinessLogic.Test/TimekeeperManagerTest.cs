using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.DataAccess;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using CGSH.ClientDashboard.BusinessEntitity;

namespace CGSH.ClientDashboard.BusinessLogic.Test
{
    [TestClass]
    public class TimekeeperManagerTest
    {
        Mock<IApiKeyManager> mockApiKeyManager;
        Mock<ITimekeeperDataAccess> mockTimekeeperDataAccess;
        Mock<IMemCache> mockMemCache;
        Mock<ICustomExceptionManager> mockCustomExceptionManager;

       [TestInitialize]
        public void SetupMethod()
        {
            mockApiKeyManager = new Mock<IApiKeyManager>();
            mockTimekeeperDataAccess = new Mock<ITimekeeperDataAccess>();
            mockMemCache = new Mock<IMemCache>();
            mockCustomExceptionManager = new Mock<ICustomExceptionManager>();
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task Timekeeper_Get_InvalidApiKey_return_null()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(false));
            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(false);

            TimekeeperManager timekeeperMgr = new TimekeeperManager(mockApiKeyManager.Object, mockTimekeeperDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result = await timekeeperMgr.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);

        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        [ExpectedException(typeof(Exception))]
        public async Task Timekeeper_Get_throwError()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));

            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            var fakeClients = new List<Client>();
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out fakeClients)).Returns(false);

            mockTimekeeperDataAccess.Setup(x => x.Get(It.IsAny<string>(),It.IsAny<DateTime>(), It.IsAny<DateTime>(),1)).Throws(new Exception());


           TimekeeperManager timekeeperMgr = new TimekeeperManager(mockApiKeyManager.Object, mockTimekeeperDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result = await timekeeperMgr.Get("a",It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);

        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task Timekeeper_Get_FromCache_return_ListOfEntity()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));
            var clients = new List<Person>() { new Person() { Name = "person" } };
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out clients)).Returns(true);

            TimekeeperManager timekeeperMgr = new TimekeeperManager(mockApiKeyManager.Object, mockTimekeeperDataAccess.Object, mockMemCache.Object, null);
            var results = await timekeeperMgr.Get("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);
            Assert.AreEqual("person", results[0].Name);

        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task Timekeeper_Get_InvalidRequest_Return_AllErrorList()
        {
            TimekeeperManager timekeeperMgr = new TimekeeperManager(mockApiKeyManager.Object, mockTimekeeperDataAccess.Object, mockMemCache.Object, null);
            var results = await timekeeperMgr.ValidateInputParams("", "", DateTime.Parse("01-01-2016"), DateTime.Parse("01-01-2016"), 0);
            StringAssert.Contains(results, "apiKey");
            StringAssert.Contains(results, "clientGroupNumber");
            StringAssert.Contains(results, "startDate");
            StringAssert.Contains(results, "threshold");
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task Timekeeper_Get_ValidRequest_Return_NoError()
        {
            TimekeeperManager timekeeperMgr = new TimekeeperManager(mockApiKeyManager.Object, mockTimekeeperDataAccess.Object, mockMemCache.Object, null);
            var results = await timekeeperMgr.ValidateInputParams("abc", "cde", DateTime.Parse("01-01-2016"), DateTime.Parse("01-02-2016"), 1);
            Assert.AreEqual(results, "");
            
        }

    }
}
