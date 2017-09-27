using System;
using System.Threading.Tasks;
using EazeCodingExercise.Contracts.Events;
using EazeCodingExercise.Endpoint.Services;
using EazeCodingExercise.Repo;
using NServiceBus;

namespace EazeCodingExercise.Endpoint.Handlers
{
    public class WebScrapRequestedHandler : IHandleMessages<IWebScrapRequested>
    {
        private readonly IWebScrapService _webScrapService;
        private readonly IWebScrapRepo _webScrapRepo;

        public WebScrapRequestedHandler(IWebScrapRepo webScrapRepo, IWebScrapService webScrapService)
        {
            if (webScrapRepo == null) throw new ArgumentNullException(nameof(webScrapRepo));
            if (webScrapService == null) throw new ArgumentNullException(nameof(webScrapService));

            _webScrapRepo = webScrapRepo;
            _webScrapService = webScrapService;
        }

        public async Task Handle(IWebScrapRequested message, IMessageHandlerContext context)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var webScrap = ToWebScrap(message);
            await _webScrapRepo.UpdateAsync(webScrap).ConfigureAwait(false);

            try
            {
                webScrap.Response = await _webScrapService.GetHtmlAsync(message.Uri, message.Xpath).ConfigureAwait(false);
                webScrap.Status = "Complete.";
            }
            catch (Exception e)
            {
                webScrap.JobError = e.Message;
                webScrap.Status = "Errored.";
            }

            await _webScrapRepo.UpdateAsync(webScrap).ConfigureAwait(false);
        }

        private WebScrap ToWebScrap(IWebScrapRequested webScrapRequested)
        {
            return new WebScrap
            {
                JobId = webScrapRequested.JobId,
                Status = "In progress",
                JobError = null,
                Response = null
            };
        }
    }
}
