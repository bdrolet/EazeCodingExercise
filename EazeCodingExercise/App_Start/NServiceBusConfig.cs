using Autofac;
using Autofac.Integration.WebApi;
using NServiceBus;
using System.Diagnostics;
using System.Reflection;
using log4net.Config;
using NServiceBus.Features;

namespace EazeCodingExercise.App_Start
{
    public static class NServiceBusConfig
    {
        public static IEndpointInstance SetupEndpoint()
        {
            var configuration = new EndpointConfiguration(WebApiConfig.ServiceName);

            XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerApiRequest();
            configuration.UseContainer<AutofacBuilder>(container => container.ExistingLifetimeScope(builder.Build()));

            configuration.UseTransport<RabbitMQTransport>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();

            configuration.PurgeOnStartup(false);

            if (Debugger.IsAttached)
            {
                configuration.EnableInstallers();
            }

            configuration.Conventions()
                .DefiningEventsAs(type => type.Namespace != null && type.Namespace.StartsWith("EazeCodingExercise.") && type.Namespace.EndsWith(".Events"));

            configuration.SendOnly();

            return Endpoint.Start(configuration).GetAwaiter().GetResult();
        }
    }
}