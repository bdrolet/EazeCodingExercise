using System;
using System.Threading.Tasks;
using EazeCodingExercise.Contracts.Events;
using EazeCodingExercise.Controllers;
using EazeCodingExercise.Models;
using EazeCodingExercise.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus;

namespace EazeCodingExercise.Tests
{
    [TestClass]
    public class WebScrapControllerTests
    {
        private Mock<IEndpointInstance> _endpointInstanceMock;
        private Mock<IWebScrapRepo> _webScrapRepoMock;
        private WebScrapController _webScrapController;

        private readonly WebScrapRequest _webScrapRequest = new WebScrapRequest
        {
            Uri = new Uri("http://someurl.com"),
            Xpath = "/html",
        };
        private readonly WebScrap _webScrap = new WebScrap
        {
            JobId = Guid.NewGuid(),
            Status = "Queued.",
            JobError = null,
            Response = null,
        };

        [TestInitialize]
        public void TestInit()
        {
            _endpointInstanceMock = new Mock<IEndpointInstance>(MockBehavior.Loose);
            _webScrapRepoMock = new Mock<IWebScrapRepo>(MockBehavior.Loose);
            _webScrapController = new WebScrapController(_endpointInstanceMock.Object, _webScrapRepoMock.Object);

            _webScrapRepoMock.Setup(repo => repo.GetAsync(_webScrap.JobId))
                .ReturnsAsync(_webScrap);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _endpointInstanceMock = null;
            _webScrapRepoMock = null;
            _webScrapController = null;
        }

        [TestMethod]
        public async Task Post_VerifyCalls()
        {
            await _webScrapController.Post(_webScrapRequest);

            _webScrapRepoMock.Verify(repo => repo.InsertAsync(It.IsAny<WebScrap>()), Times.Once);
            _endpointInstanceMock.Verify(endpoint => endpoint.Publish<IWebScrapRequested>(It.IsAny<Action<IWebScrapRequested>>(), It.IsAny<PublishOptions>()), Times.Once);
        }

        [TestMethod]
        public async Task Post_VerifyResponse()
        {
            var response = await _webScrapController.Post(_webScrapRequest);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(response, new Guid());
        }

        [TestMethod]
        public async Task Get_VerifyResponse()
        {
            var response = await _webScrapController.Get(_webScrap.JobId);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.JobId, _webScrap.JobId);
            Assert.AreEqual(response.JobError, _webScrap.JobError);
            Assert.AreEqual(response.Response, _webScrap.Response);
            Assert.AreEqual(response.Status, _webScrap.Status);
        }
    }
}
