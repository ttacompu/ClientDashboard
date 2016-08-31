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
    public class SearchManagerTest
    {
        Mock<IApiKeyManager> mockApiKeyManager;
        Mock<ISearchDataAccess> mockSearchDataAccess;
        Mock<IMemCache> mockMemCache;
        Mock<ICustomExceptionManager> mockCustomExceptionManager;

        [TestInitialize]
        public void SetupMethod()
        {
            mockApiKeyManager = new Mock<IApiKeyManager>();
            mockSearchDataAccess = new Mock<ISearchDataAccess>();
            mockMemCache = new Mock<IMemCache>();
            mockCustomExceptionManager = new Mock<ICustomExceptionManager>();
        }

        [TestMethod]
        [TestCategory("Search")]
        public async Task SearchManager_Get_InvalidApiKey_return_null()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(false));
            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(false);

            SearchManager searchMgr = new SearchManager(mockApiKeyManager.Object, mockSearchDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result = await searchMgr.Get(It.IsAny<string>(), It.IsAny<string>());

        }

        [TestMethod]
        [TestCategory("Search")]
        [ExpectedException(typeof(Exception))]
        public async Task SearchManager_Get_throwError()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));

            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            var fakeClients = new List<Client>();
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out fakeClients)).Returns(false);

            mockSearchDataAccess.Setup(x => x.Get(It.IsAny<string>())).Throws(new Exception());


            SearchManager searchMgr = new SearchManager(mockApiKeyManager.Object, mockSearchDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result = await searchMgr.Get(It.IsAny<string>(), It.IsAny<string>());

        }

        [TestMethod]
        [TestCategory("Search")]
        public async Task SearchManager_Get_FromCache_return_ListOfEntity()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));
            var clients = new List<ClientGroup>() { new ClientGroup() { Name = "clientGroup" } };
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out clients)).Returns(true);

            SearchManager searchMgr = new SearchManager(mockApiKeyManager.Object, mockSearchDataAccess.Object, mockMemCache.Object, null);
            var results = await searchMgr.Get(It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual("clientGroup", results[0].Name);

        }

        [TestMethod]
        [TestCategory("Search")]
        public async Task SearchManager_Get_DB_return_ListOfEntity()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));
            var fakeClients = new List<Client>();
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out fakeClients)).Returns(false);
            mockSearchDataAccess.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(new List<ClientGroup>() { new ClientGroup() { Name = "clientGroup" } }));

            SearchManager searchMgr = new SearchManager(mockApiKeyManager.Object, mockSearchDataAccess.Object, mockMemCache.Object, null);
            var results = await searchMgr.Get(It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual("clientGroup", results[0].Name);

        }

        [TestCleanup]
        public void CleanUp()
        {
            mockApiKeyManager = null;
            mockSearchDataAccess = null;
            mockMemCache = null;
        }
    }
}
