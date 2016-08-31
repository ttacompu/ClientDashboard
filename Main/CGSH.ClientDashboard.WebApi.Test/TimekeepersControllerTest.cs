using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using CGSH.ClientDashboard.Exceptions;

namespace CGSH.ClientDashboard.WebApi.Test
{
    [TestClass]
    public class TimekeepersControllerTest
    {

        Mock<ITimekeeperManager> mockTimekeeperManager;
        [TestInitialize]
        public void SetupMethod()
        {
            mockTimekeeperManager = new Mock<ITimekeeperManager>();
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task TimekeeperController_Get_Return_List_Clients()
        {

            mockTimekeeperManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(Task.FromResult(new List<Person>() { new Person { Name = "person" } }));

            TimekeepersController timekeepersController = new TimekeepersController(mockTimekeeperManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await timekeepersController.Get("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);

            var resultsKeys = result as OkNegotiatedContentResult<List<Person>>;

            Assert.IsNotNull(resultsKeys);
            Assert.IsNotNull(resultsKeys.Content);
            Assert.AreEqual("person", resultsKeys.Content[0].Name);
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task TimekeeperController_Get_Return_List_NotFound()
        {

            mockTimekeeperManager.Setup(x => x.Get("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1)).Returns(Task.FromResult(new List<Person>() { new Person { Name = "person" } }));

            TimekeepersController timekeeperController = new TimekeepersController(mockTimekeeperManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            //First parameter passed (api key) to blank string
            var result = await timekeeperController.Get("", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        [ExpectedException(typeof(Exception))]
        public async Task TimekeeperController_Get_Return_ThrowError()
        {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            mockTimekeeperManager.Setup(x => x.Get("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1)).Throws(new Exception());
            TimekeepersController timekeeperController = new TimekeepersController(mockTimekeeperManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await timekeeperController.Get("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task TimekeeperController_Get_BadRequest() {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(false);

            mockTimekeeperManager.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()));
            mockTimekeeperManager.Setup(x => x.ValidateInputParams(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(Task.FromResult("key:value"));

            TimekeepersController timekeeperController = new TimekeepersController(mockTimekeeperManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result=await timekeeperController.Get("", "", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-01-01"), 1);
            var errorResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual("key:value", errorResult.Message);

        }

        //Counsel method
        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task TimekeeperController_GetPartnersAndSeniorCounsel_Return_List_Clients()
        {

            mockTimekeeperManager.Setup(x => x.GetPartnersAndSeniorCounsel(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(Task.FromResult(new List<Person>() { new Person { Name = "person" } }));

            TimekeepersController timekeepersController = new TimekeepersController(mockTimekeeperManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await timekeepersController.GetPartnersAndSeniorCounsel("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);

            var resultsKeys = result as OkNegotiatedContentResult<List<Person>>;

            Assert.IsNotNull(resultsKeys);
            Assert.IsNotNull(resultsKeys.Content);
            Assert.AreEqual("person", resultsKeys.Content[0].Name);
        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        public async Task TimekeeperController_GetPartnersAndSeniorCounsel_Return_List_NotFound()
        {

            mockTimekeeperManager.Setup(x => x.GetPartnersAndSeniorCounsel("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1)).Returns(Task.FromResult(new List<Person>() { new Person { Name = "person" } }));

            TimekeepersController timekeeperController = new TimekeepersController(mockTimekeeperManager.Object, (ICustomExceptionManager)null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            //First parameter passed (api key) to blank string
            var result = await timekeeperController.GetPartnersAndSeniorCounsel("", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        [TestCategory("Timekeeper")]
        [ExpectedException(typeof(Exception))]
        public async Task TimekeeperController_GetPartnersAndSeniorCounsel_Return_ThrowError()
        {
            var mockExceptionManager = new Mock<ICustomExceptionManager>();
            var fakeException = new Exception();
            mockExceptionManager.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<string>(), out fakeException)).Returns(true);

            mockTimekeeperManager.Setup(x => x.GetPartnersAndSeniorCounsel("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1)).Throws(new Exception());
            TimekeepersController timekeeperController = new TimekeepersController(mockTimekeeperManager.Object, mockExceptionManager.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = await timekeeperController.GetPartnersAndSeniorCounsel("a", It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1);
        }

    }

}
