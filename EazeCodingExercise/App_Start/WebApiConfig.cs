using System.Web.Http;

namespace EazeCodingExercise
{
    public static class WebApiConfig
    {
        public static string ServiceName = "EazeCodingExercise";

        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
