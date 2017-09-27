using System;

namespace EazeCodingExercise.Models
{
    public class WebScrapResponse
    {
        public Guid JobId { get; set; }

        public string Status { get; set; }

        public string Response { get; set; }

        public string JobError { get; set; }
    }
}