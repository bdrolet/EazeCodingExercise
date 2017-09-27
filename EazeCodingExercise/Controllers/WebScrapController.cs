using EazeCodingExercise.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using EazeCodingExercise.Contracts.Events;
using EazeCodingExercise.Repo;
using NServiceBus;

namespace EazeCodingExercise.Controllers
{
    public class WebScrapController : ApiController
    {
        private readonly IEndpointInstance _endpointInstance;
        private readonly IWebScrapRepo _webScrapRepo;

        public WebScrapController(IEndpointInstance endpointInstance, IWebScrapRepo webScrapRepo)
        {
            if (endpointInstance == null) throw new ArgumentNullException(nameof(endpointInstance));
            if (webScrapRepo == null) throw new ArgumentNullException(nameof(webScrapRepo));

            _endpointInstance = endpointInstance;
            _webScrapRepo = webScrapRepo;
        }
        public async Task<WebScrapResponse> Get(Guid id)
        {
            var webScrap = await _webScrapRepo.GetAsync(id).ConfigureAwait(false);
            return new WebScrapResponse
            {
                JobId = webScrap.JobId,
                Response = webScrap.Response,
                JobError = webScrap.JobError,
                Status = webScrap.Status,
            };
        }

        public async Task<Guid> Post(WebScrapRequest request)
        {
            Guid jobId = Guid.NewGuid();

            var webScrap = new WebScrap
            {
                JobId = jobId,
                Status = "Queued.",
                Response = null,
                JobError = null,
            };

            await _webScrapRepo.InsertAsync(webScrap).ConfigureAwait(false);

            await _endpointInstance.Publish<IWebScrapRequested>(message =>
            {
                message.JobId = jobId;
                message.Uri = request.Uri;
                message.Xpath = request.Xpath;
            }).ConfigureAwait(false);

            return jobId;
        }
    }
}
