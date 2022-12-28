namespace Auxquimia
{
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NLog.Web;
    using System;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        public static void Main(string[] args)
        {
            Program.args = args;
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Defines the args
        /// </summary>
        private static string[] args;

        /// <summary>
        /// The CreateWebHostBuilder
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        /// <returns>The <see cref="IWebHostBuilder"/></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddConfiguration)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();

        /// <summary>
        /// The AddConfiguration
        /// </summary>
        /// <param name="context">The context<see cref="WebHostBuilderContext"/></param>
        /// <param name="builder">The builder<see cref="IConfigurationBuilder"/></param>
        private static void AddConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //context.HostingEnvironment.EnvironmentName = "local"; //Pruebas ejecutable
            builder.Sources.Clear();
            builder
                .AddYamlFile(path: "application.yml", optional: false, reloadOnChange: true)
                .AddYamlFile(path: $"application-{context.HostingEnvironment.EnvironmentName}.yml", optional: true, reloadOnChange: true)
                .AddYamlFile(path: Path.Combine(userHome, $"application.{context.HostingEnvironment.ApplicationName}.yml"), optional: true, reloadOnChange: true)
                .AddYamlFile(path: Path.Combine(userHome, $"application.{context.HostingEnvironment.ApplicationName}-{context.HostingEnvironment.EnvironmentName}.yml"),
                             optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "APP_");

            NLog.LogManager.LoadConfiguration("nlog.config");

            if (File.Exists($"nlog-{context.HostingEnvironment.EnvironmentName}.config"))
            {
                NLog.LogManager.LoadConfiguration($"nlog-{context.HostingEnvironment.EnvironmentName}.config");
            }
            if (File.Exists(Path.Combine(userHome, $"nlog.{context.HostingEnvironment.ApplicationName}.config")))
            {
                NLog.LogManager.LoadConfiguration(Path.Combine(userHome, $"nlog.{context.HostingEnvironment.ApplicationName}.config"));
            }
            if (File.Exists(Path.Combine(userHome, $"nlog.{context.HostingEnvironment.ApplicationName}-{context.HostingEnvironment.EnvironmentName}.config")))
            {
                NLog.LogManager.LoadConfiguration(Path.Combine(userHome, $"nlog.{context.HostingEnvironment.ApplicationName}-{context.HostingEnvironment.EnvironmentName}.config"));
            }

            if (args != null)
            {
                builder.AddCommandLine(args);
            }
        }
    }
}
