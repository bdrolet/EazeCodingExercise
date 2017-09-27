using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;

namespace EazeCodingExercise.App_Start
{
    public static class AutofacConfig
    {
        public static ContainerBuilder GetAutofacContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            return builder;
        }
    }
}