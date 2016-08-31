using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
namespace CGSH.ClientDashboard.WebApi.Test
{
    [TestClass]
    public class SearchControllerTest
    {

        Mock<ISearchManager> mockSearchManager;
        [TestInitialize]
        public void SetupMethod()
        {
            mockSearchManager = new Mock<ISearchManager>();
        }

        [TestMethod]
        [TestCategory("Search")]
        public async Task SearchController_Get_Return_List_Clients()
        {

            mockSearchManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<ClientGroup>() { new ClientGroup { Name = "clientGroup" } }));

            SearchController searchController = new SearchController(mockSearchManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await searchController.Get("aa", "bb");

            var resultsKeys = result as OkNegotiatedContentResult<List<ClientGroup>>;

            Assert.IsNotNull(resultsKeys);
            Assert.IsNotNull(resultsKeys.Content);
            Assert.AreEqual("clientGroup", resultsKeys.Content[0].Name);
        }

        [TestMethod]
        [TestCategory("Search")]
        public async Task SearchController_Get_Return_List_NotFound()
        {

            mockSearchManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<ClientGroup>() { new ClientGroup { Name = "clientGroup" } }));

            SearchController searchController = new SearchController(mockSearchManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await searchController.Get("", "");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        [TestCategory("Search")]
        [ExpectedException(typeof(Exception))]
        public async Task SearchController_All_Return_ThrowError()
        {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            mockSearchManager.Setup(x => x.Get("a", "b")).Throws(new Exception());
            SearchController searchController = new SearchController(mockSearchManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await searchController.Get("a", "b");
        }
    }
}
