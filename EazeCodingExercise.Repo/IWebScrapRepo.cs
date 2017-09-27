using System;
using System.Threading.Tasks;

namespace EazeCodingExercise.Repo
{
    public interface IWebScrapRepo
    {
        Task<WebScrap> GetAsync(Guid id);
        Task InsertAsync(WebScrap webScrap);
        Task UpdateAsync(WebScrap webScrap);
    }
}