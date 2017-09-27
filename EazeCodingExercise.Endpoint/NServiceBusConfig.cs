using Autofac;
using EazeCodingExercise.Endpoint.Services;
using EazeCodingExercise.Repo;
using log4net.Config;
using NServiceBus;
using NServiceBus.Features;

namespace EazeCodingExercise.Endpoint
{
    public static class NServiceBusConfig
    {
        public static string ServiceName = "EazeCodingExercise";

        public static IEndpointInstance SetupEndpoint()
        {
            var configuration = new EndpointConfiguration(ServiceName);

            XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(MongoConfig.WebScrapCollection);
            builder.RegisterType<WebScrapRepo>().AsImplementedInterfaces();
            builder.RegisterType<WebScrapService>().AsImplementedInterfaces();

            configuration.UseContainer<AutofacBuilder>(container => container.ExistingLifetimeScope(builder.Build()));

            configuration.UseTransport<RabbitMQTransport>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.PurgeOnStartup(false);
            configuration.EnableInstallers();

            configuration.Conventions()
                .DefiningEventsAs(type => type.Namespace != null && type.Namespace.StartsWith("EazeCodingExercise.") && type.Namespace.EndsWith(".Events"));

            return NServiceBus.Endpoint.Start(configuration).GetAwaiter().GetResult();
        }
    }
}