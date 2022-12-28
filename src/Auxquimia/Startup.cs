namespace Auxquimia
{
    using Autofac;
    using AutoMapper;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Auxquimia.Filters;
    using System;
    using System.Reflection;
    using Auxquimia.Module;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.OpenApi.Models;
    using Auxquimia.Utils.Kafka;

    /// <summary>
    /// Defines the <see cref="Startup" />
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the ApplicationContainer
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceProvider"/></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            CorsConfig.ConfigureServices(services, Configuration);
            CacheConfig.ConfigureServices(services, Configuration);

            services
                .AddMvc(opt =>
                {
                    opt.Filters.Add(new ApiExceptionFilterAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit
                // configuration.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Automatización del proceso productivo (Auxquimia)"
                });
            });
            
            services.AddAutoMapper(Assembly.GetAssembly(typeof(ServiceModule)), Assembly.GetAssembly(typeof(Izertis.Paging.Abstractions.PagingProfile)));

            IdentityServerConfig.ConfigureServices(services, Configuration);
            ApplicationContainer = AutofacConfig.ConfigureServices(services);

            return new AutofacServiceProvider(ApplicationContainer);
        }

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        /// <param name="appLifetime">The appLifetime<see cref="IApplicationLifetime"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsEnvironment(Environments.Local))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
                app.UseHsts();
            }

            CorsConfig.Configure(app, env, Configuration);
            app.UsePreserveCors();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "API Automatización del proceso productivo");
            });

            app.UseMvc();

            IdentityServerConfig.Configure(app, env);


            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
