using System;
using System.Threading.Tasks;

namespace EazeCodingExercise.Endpoint.Services
{
    public interface IWebScrapService
    {
        Task<string> GetHtmlAsync(Uri uri, string xPath);
    }
}