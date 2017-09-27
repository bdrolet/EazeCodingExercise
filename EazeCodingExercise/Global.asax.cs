using Autofac;
using Autofac.Integration.WebApi;
using EazeCodingExercise.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using EazeCodingExercise.Repo;

namespace EazeCodingExercise
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var endpoint = NServiceBusConfig.SetupEndpoint();

            var builder = AutofacConfig.GetAutofacContainerBuilder();
            builder.RegisterInstance(endpoint);
            builder.RegisterInstance(MongoConfig.WebScrapCollection);
            builder.RegisterType<WebScrapRepo>().AsImplementedInterfaces();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
