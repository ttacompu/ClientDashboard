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
    public class ApiKeyManagerTest
    {

        Mock<IApiKeyDataService> mockApiKeyDataService;
        Mock<ICustomExceptionManager> mockCustomExceptionManager;

        [TestInitialize]
        public void SetupMethod()
        {
            mockApiKeyDataService = new Mock<IApiKeyDataService>();
            mockCustomExceptionManager = new Mock<ICustomExceptionManager>();
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_Get_InvalidApiKey_return_false()
        {
            mockApiKeyDataService.Setup(x => x.All()).Returns(Task.FromResult(new List<ApiKey>() { }));


            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.IsValid("1");
            Assert.AreEqual(false, result);

        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_ValidApiKey_return_true()
        {
            mockApiKeyDataService.Setup(x => x.All()).Returns(Task.FromResult(new List<ApiKey>() { new ApiKey { Key = "1" } }));
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.IsValid("1");
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_All_return_ListOfApiKeys()
        {
            mockApiKeyDataService.Setup(x => x.All()).Returns(Task.FromResult(new List<ApiKey>() { new ApiKey { Key = "1" } }));
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.All();
            Assert.AreEqual(1, result.Count);

        }

        [TestMethod]
        [TestCategory("ApiKey")]
        [ExpectedException(typeof(Exception))]
        public async Task ApiKeyManager_All_return_throwException()
        {
            mockApiKeyDataService.Setup(x => x.All()).Throws(new Exception());
            var fakeException = new Exception();
            mockCustomExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.All();
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_Get_return_OneApiKey()
        {
            mockApiKeyDataService.Setup(x => x.Get(1)).Returns(Task.FromResult(new ApiKey() { Key ="key" }));
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.Get(1);
            Assert.AreEqual("key", result.Key);

        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_Delete_return_true()
        {
            mockApiKeyDataService.Setup(x => x.Delete(1)).Returns(Task.FromResult(true));
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.Delete(1);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyManager_Save_return_ApiKey()
        {
            mockApiKeyDataService.Setup(x => x.Save(It.IsAny<ApiKey>())).Returns(Task.FromResult(new ApiKey() { Key="key"}));
            ApiKeyManager apiMgr = new ApiKeyManager(mockApiKeyDataService.Object, mockCustomExceptionManager.Object);
            var result = await apiMgr.Save(It.IsAny<ApiKey>());
            Assert.AreEqual("key", result.Key);
        }
    }
}
