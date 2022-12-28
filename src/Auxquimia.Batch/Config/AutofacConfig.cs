namespace Auxquimia.Batch.Config
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Auxquimia.Batch.Infraestructure;
    using Auxquimia.Module;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// Defines the <see cref="AutofacConfig" />.
    /// </summary>
    public static class AutofacConfig
    {
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="containerBuilder">The containerBuilder<see cref="ContainerBuilder"/>.</param>
        public static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Processor"))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .AsSelf()
                   .InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Reader"))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .AsSelf()
                   .InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Writer"))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Runner"))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            containerBuilder.RegisterModule<ServiceModule>();
        }
    }
}
