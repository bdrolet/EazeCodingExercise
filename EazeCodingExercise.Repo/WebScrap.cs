using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EazeCodingExercise.Repo
{
    public class WebScrap
    {
        [BsonId]
        public Guid JobId { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("response")]
        public string Response { get; set; }

        [BsonElement("jobError")]
        public string JobError { get; set; }
    }
}