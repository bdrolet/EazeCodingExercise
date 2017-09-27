using System;

namespace EazeCodingExercise.Contracts.Events
{
    public interface IWebScrapRequested
    {
        Guid JobId { get; set; }
        string Xpath { get; set; }
        Uri Uri { get; set; }
    }
}
