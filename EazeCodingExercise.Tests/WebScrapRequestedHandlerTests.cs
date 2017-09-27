using System;
using System.Threading.Tasks;
using EazeCodingExercise.Contracts.Events;
using EazeCodingExercise.Endpoint.Handlers;
using EazeCodingExercise.Endpoint.Services;
using EazeCodingExercise.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus.Testing;

namespace EazeCodingExercise.Tests
{
    [TestClass]
    public class WebScrapRequestedHandlerTests
    {
        private Mock<IWebScrapService> _webScrapServiceMock;
        private Mock<IWebScrapRepo> _webScrapRepoMock;

        private readonly Guid _jobId = Guid.NewGuid();
        private readonly Uri _uri = new Uri("http://someurl");
        private const string Xpath = "/html";
        private readonly string _result = "This value.";

        [TestInitialize]
        public void TestInit()
        {
            _webScrapServiceMock = new Mock<IWebScrapService>(MockBehavior.Loose);
            _webScrapRepoMock = new Mock<IWebScrapRepo>(MockBehavior.Loose);

            _webScrapServiceMock.Setup(service => service.GetHtmlAsync(_uri, Xpath))
                .ReturnsAsync(Xpath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _webScrapRepoMock = null;
            _webScrapServiceMock = null;
        }

        [TestMethod]
        public void Handle_VerifyCalls()
        {
            Test.Handler(new WebScrapRequestedHandler(_webScrapRepoMock.Object, _webScrapServiceMock.Object))
                .OnMessage<IWebScrapRequested>(request =>
                {
                    request.JobId = _jobId;
                    request.Uri = _uri;
                    request.Xpath = Xpath;
                });

            _webScrapServiceMock.Verify();
        }
    }
}
