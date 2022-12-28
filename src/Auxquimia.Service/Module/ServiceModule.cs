namespace Auxquimia.Module
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Extras.DynamicProxy;
    using Auxquimia.Config;
    using Auxquimia.Utils;
    using Auxquimia.Utils.AutoFac;
    using Izertis.Caching.Utilities;
    using Izertis.NHibernate.Repositories;
    using Izertis.NHibernate.Repositories.Interceptors;

    /// <summary>
    /// Defines the <see cref="ServiceModule" />
    /// </summary>
    public class ServiceModule : Module
    {
        /// <summary>
        /// Defines the sessionFactoryProviderName
        /// !!! MUST BE UNIQUE !!!
        /// </summary>
        private static readonly string SESSION_FACTORY_PROVIDER_NAME = "appSessionFactoryProvider";

        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="builder">The builder<see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterType<AsyncRunner>().As<IAsyncRunner>().SingleInstance();

            builder.RegisterType<FluentNhibernateLocalSessionFactoryProvider>()
                .WithParameter("configSectionName", "database:app")
                .AsImplementedInterfaces().AsSelf().SingleInstance().AutoActivate()
                .Named<IFluentNhibernateLocalSessionFactoryProvider>(SESSION_FACTORY_PROVIDER_NAME);

            builder.RegisterType<AsyncTransactionInterceptor>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IFluentNhibernateLocalSessionFactoryProvider),
                    (pi, ctx) => ctx.ResolveNamed<IFluentNhibernateLocalSessionFactoryProvider>(SESSION_FACTORY_PROVIDER_NAME)))
                .AsImplementedInterfaces().AsSelf();

            builder.RegisterType<AsyncDeterminationGenericInterceptor<AsyncTransactionInterceptor>>();

            builder.RegisterType<AsyncCacheInterceptor>()
                .AsImplementedInterfaces().AsSelf();

            builder.RegisterType<AsyncCacheDeterminationInterceptor<AsyncCacheInterceptor>>();

            builder.RegisterType<MapperUtilsBootstrapper>().AsSelf().SingleInstance().AutoActivate();

            builder.RegisterType<ContextConfigProvider>()
                .AsImplementedInterfaces().AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(AsyncCacheDeterminationInterceptor<AsyncCacheInterceptor>))
                   .InterceptedBy(typeof(AsyncDeterminationGenericInterceptor<AsyncTransactionInterceptor>))
                   .AsImplementedInterfaces().PropertiesAutowired()
                   .InstancePerLifetimeScope();

            ////Auxquimia Service provider
            builder.RegisterType<AuxquimiaServiceProvider>()
                .AsImplementedInterfaces().AsSelf().SingleInstance().AutoActivate();
        }
    }
}
