namespace Auxquimia.Batch
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using Auxquimia.Batch.Config;
    using Auxquimia.Module;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The CompositionRoot.
        /// </summary>
        /// <returns>The <see cref="IContainer"/>.</returns>
        static private IContainer CompositionRoot()
        {
            string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(environment))
            {
                environment = "local";
            }

            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            string applicationName = currentAssembly.GetName().Name;
            IConfiguration config = InitializeConfiguration(userHome, environment, applicationName);

            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                });


            services.AddAutoMapper(Assembly.GetAssembly(typeof(ServiceModule)), Assembly.GetAssembly(typeof(Izertis.Paging.Abstractions.PagingProfile)));
            services.AddCacheManager(config);


            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterType<Application>();
            containerBuilder.RegisterInstance(config).AsImplementedInterfaces().AsSelf();

            AutofacConfig.ConfigureServices(containerBuilder);

            return containerBuilder.Build();
        }

        /// <summary>
        /// The InitializeConfiguration.
        /// </summary>
        /// <param name="userHome">The userHome<see cref="string"/>.</param>
        /// <param name="environment">The environment<see cref="string"/>.</param>
        /// <param name="applicationName">The applicationName<see cref="string"/>.</param>
        /// <returns>The <see cref="IConfiguration"/>.</returns>
        private static IConfiguration InitializeConfiguration(string userHome, string environment, string applicationName)
        {
            return new ConfigurationBuilder()
               .AddYamlFile(path: "application.yml", optional: false, reloadOnChange: true)
               .AddYamlFile(path: $"application-{environment}.yml", optional: true, reloadOnChange: true)
               .AddYamlFile(path: Path.Combine(userHome, $"application.{applicationName}.yml"), optional: true, reloadOnChange: true)
               .AddYamlFile(path: Path.Combine(userHome, $"application.{applicationName}-{environment}.yml"),
                             optional: true, reloadOnChange: true)
               .Build();
        }

        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        internal static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The MainAsync.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        internal static async Task MainAsync(string[] args)
        {
            int result = await CompositionRoot().Resolve<Application>().Run();

            Environment.Exit(result);
        }
    }
}
