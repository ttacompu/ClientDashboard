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
    public class EntityManagerTest
    {
        Mock<IApiKeyManager> mockApiKeyManager;
        Mock<IEntityDataAccess> mockEntityDataAccess;
        Mock<IMemCache> mockMemCache;
        Mock<ICustomExceptionManager> mockCustomExceptionManager;

        [TestInitialize]
        public void SetupMethod()
        {
            mockApiKeyManager = new Mock<IApiKeyManager>();
            mockEntityDataAccess = new Mock<IEntityDataAccess>();
            mockMemCache = new Mock<IMemCache>();
            mockCustomExceptionManager = new Mock<ICustomExceptionManager>();
        }

        [TestMethod]
        [TestCategory("Entity")]
        public async Task EntityManager_Get_InvalidApiKey_return_null() {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(false));
            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(false);

            EntityManager entMgr = new EntityManager(mockApiKeyManager.Object, mockEntityDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result=await entMgr.Get(It.IsAny<string>(), It.IsAny<string>());

        }

        [TestMethod]
        [TestCategory("Entity")]
        [ExpectedException(typeof(Exception))]
        public async Task EntityManager_Get_throwError()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));

            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            var fakeClients = new List<Client>();
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out fakeClients)).Returns(false);

            mockEntityDataAccess.Setup(x => x.Get(It.IsAny<string>())).Throws(new Exception());


            EntityManager entMgr = new EntityManager(mockApiKeyManager.Object, mockEntityDataAccess.Object, mockMemCache.Object, mockCustomExceptionManager.Object);
            var result = await entMgr.Get(It.IsAny<string>(), It.IsAny<string>());

        }

        [TestMethod]
        [TestCategory("Entity")]
        public async Task EntityManager_Get_FromCache_return_ListOfEntity()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));
            var clients = new List<Client>() { new Client() { Name="client"} };
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out clients)).Returns(true);

            EntityManager entMgr = new EntityManager(mockApiKeyManager.Object, mockEntityDataAccess.Object, mockMemCache.Object, null);
            var results = await entMgr.Get(It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual("client", results[0].Name);

        }

        [TestMethod]
        [TestCategory("Entity")]
        public async Task EntityManager_Get_DB_return_ListOfEntity()
        {
            mockApiKeyManager.Setup(x => x.IsValid(It.IsAny<string>())).Returns(Task.FromResult(true));
            var fakeClients = new List<Client>();
            mockMemCache.Setup(x => x.TryGet(It.IsAny<string>(), out fakeClients)).Returns(false);
            mockEntityDataAccess.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(new List<Client>() { new Client() {Name ="client" } }));

            EntityManager entMgr = new EntityManager(mockApiKeyManager.Object, mockEntityDataAccess.Object, mockMemCache.Object, null);
            var results = await entMgr.Get(It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual("client", results[0].Name);

        }

        [TestCleanup]
        public void CleanUp()
        {
            mockApiKeyManager = null;
            mockEntityDataAccess = null;
            mockMemCache = null;
            mockCustomExceptionManager = null;
        }
    }
}
