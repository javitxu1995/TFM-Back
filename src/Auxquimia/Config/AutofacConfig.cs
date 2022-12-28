namespace Auxquimia
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Auxquimia.Module;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the <see cref="AutofacConfig" />
    /// </summary>
    public static class AutofacConfig
    {
        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IContainer"/></returns>
        public static Autofac.IContainer ConfigureServices(IServiceCollection services)
        {
            // Create the container builder.
            var builder = new Autofac.ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container.
            //
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.Populate(services);
            builder.RegisterModule<Modules.ApiModule>();

            return builder.Build();
        }
    }
}
