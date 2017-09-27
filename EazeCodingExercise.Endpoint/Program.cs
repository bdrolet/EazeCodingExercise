using System;

namespace EazeCodingExercise.Endpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            NServiceBusConfig.SetupEndpoint();
            Console.ReadLine();
        }
    }
}
