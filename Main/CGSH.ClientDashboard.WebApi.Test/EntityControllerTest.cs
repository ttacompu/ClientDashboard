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
    public class EntityControllerTest
    {

        Mock<IEntityManager> mockEntityManager;
        [TestInitialize]
        public void SetupMethod()
        {
            mockEntityManager = new Mock<IEntityManager>();
        }

        [TestMethod]
        [TestCategory("Entity")]
        public async Task EntityController_Get_Return_List_Clients()
        {

            mockEntityManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<Client>() { new Client { Name="client" } }));

            EntitiesController entityController = new EntitiesController(mockEntityManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await entityController.Get("aa", "bb");

            var resultsKeys = result as OkNegotiatedContentResult<List<Client>>;

            Assert.IsNotNull(resultsKeys);
            Assert.IsNotNull(resultsKeys.Content);
            Assert.AreEqual("client", resultsKeys.Content[0].Name);
        }

        [TestMethod]
        [TestCategory("Entity")]
        public async Task EntityController_Get_Return_List_NotFound()
        {

            mockEntityManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<Client>() { new Client { Name = "client" } }));

            EntitiesController entityController = new EntitiesController(mockEntityManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await entityController.Get("", "");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        [TestCategory("Entity")]
        [ExpectedException(typeof(Exception))]
        public async Task EntityController_All_Return_ThrowError()
        {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            mockEntityManager.Setup(x => x.Get("a", "b")).Throws(new Exception());
            EntitiesController entityController = new EntitiesController(mockEntityManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await entityController.Get("a","b");
        }
    }
}
