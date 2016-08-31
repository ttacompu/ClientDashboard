using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace CGSH.ClientDashboard.WebApi.Test
{
    [TestClass]
    
    public class ApiKeyControllerTest
    {
        Mock<IApiKeyManager> mockApiKeyManager;
        [TestInitialize]
        public void SetupMethod()
        {
            mockApiKeyManager = new Mock<IApiKeyManager>();
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_All_Return_List_ApiKeys()
        {

            mockApiKeyManager.Setup(x => x.All()).Returns(Task.FromResult(new List<ApiKey>() { new ApiKey {ApplicationName="FirstApp" } }));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await apiController.All();

            var resultsKeys = result as OkNegotiatedContentResult<List<ApiKey>>;

            Assert.IsNotNull(resultsKeys);
            Assert.IsNotNull(resultsKeys.Content);
            Assert.AreEqual("FirstApp", resultsKeys.Content[0].ApplicationName);
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_All_Return_NotFound()
        {
            mockApiKeyManager.Setup(x => x.All()).Returns(Task.FromResult((List<ApiKey>)null));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await apiController.All();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        [ExpectedException(typeof(Exception))]
        public async Task ApiKeyController_All_Return_ThrowError()
        {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            mockApiKeyManager.Setup(x => x.All()).Throws(new Exception());
            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await apiController.All();
        }


        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Single_Return_ApiKey()
        {
            mockApiKeyManager.Setup(x => x.Get(It.IsAny<long>())).Returns(Task.FromResult(new ApiKey {Id=1 }));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await apiController.GetByID(1);

            var resultsKey = result as OkNegotiatedContentResult<ApiKey>;

            Assert.IsNotNull(resultsKey);
            Assert.IsNotNull(resultsKey.Content);
            Assert.AreEqual(1, resultsKey.Content.Id);
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Single_Return_NotFound()
        {
            mockApiKeyManager.Setup(x => x.Get(It.IsAny<long>())).Returns(Task.FromResult((ApiKey)null));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await apiController.GetByID(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Save_Return_Default_Route_Value()
        {
            mockApiKeyManager.Setup(x => x.Save(It.IsAny<ApiKey>())).Returns(Task.FromResult(new ApiKey {Id=1 }));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = await apiController.Save(new ApiKey { });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<ApiKey>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(1, (long)createdResult.RouteValues["id"]);
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Save_Conflict()
        {
            mockApiKeyManager.Setup(x => x.Save(It.IsAny<ApiKey>())).Returns(Task.FromResult((ApiKey)null));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = await apiController.Save(new ApiKey { });
            Assert.IsInstanceOfType(actionResult, typeof(ConflictResult));
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Update_Return_Accpeted()
        {
            mockApiKeyManager.Setup(x => x.Save(It.IsAny<ApiKey>())).Returns(Task.FromResult(new ApiKey { Id = 1 }));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = await apiController.Update(new ApiKey { });
            var updatedResult = actionResult as NegotiatedContentResult<ApiKey>;

            Assert.IsNotNull(updatedResult);
            Assert.AreEqual(HttpStatusCode.Accepted, updatedResult.StatusCode);
            Assert.IsNotNull(updatedResult.Content);
            Assert.AreEqual(1, updatedResult.Content.Id);
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Update_Return_NotFound()
        {
            mockApiKeyManager.Setup(x => x.Save(It.IsAny<ApiKey>())).Returns(Task.FromResult((ApiKey)null));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = await apiController.Update(new ApiKey { });
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Deleted_Return_OKMessage()
        {
            mockApiKeyManager.Setup(x => x.Delete(It.IsAny<long>())).Returns(Task.FromResult(true));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = (dynamic)await apiController.Delete(It.IsAny<long>());
            

            Assert.IsNotNull(actionResult);
            Assert.AreEqual("api id 0 is deleted", actionResult.Content.message);
            
        }

        [TestMethod]
        [TestCategory("ApiKey")]
        public async Task ApiKeyController_Deleted_Return_NotFound()
        {
            mockApiKeyManager.Setup(x => x.Delete(It.IsAny<long>())).Returns(Task.FromResult(false));

            ApiKeyController apiController = new ApiKeyController(mockApiKeyManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var actionResult = (dynamic)await apiController.Delete(It.IsAny<long>());


            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));

        }

        [TestCleanup]
        public void CleanUp()
        {
            mockApiKeyManager = null;
        }
    }
}
