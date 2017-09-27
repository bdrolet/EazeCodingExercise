using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EazeCodingExercise.Repo
{
    public class WebScrapRepo : IWebScrapRepo
    {
        private readonly IMongoCollection<WebScrap> _collection;
        public WebScrapRepo(IMongoCollection<WebScrap> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            _collection = collection;
        }

        public async Task<WebScrap> GetAsync(Guid id)
        {
            return await _collection.Find(x => x.JobId == id).FirstAsync().ConfigureAwait(false);
        }

        public async Task InsertAsync(WebScrap webScrap)
        {
            await _collection.InsertOneAsync(webScrap).ConfigureAwait(false);
        }

        public async Task UpdateAsync(WebScrap webScrap)
        {
            var update = Builders<WebScrap>.Update.Set(x => x.JobError, webScrap.JobError)
                                                  .Set(x => x.Response, webScrap.Response)
                                                  .Set(x => x.Status, webScrap.Status);

            await _collection.UpdateOneAsync(x => x.JobId == webScrap.JobId, update).ConfigureAwait(false);
        }
    }
}
