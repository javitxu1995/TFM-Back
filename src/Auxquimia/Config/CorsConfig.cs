namespace Auxquimia
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="CorsConfig" />
    /// </summary>
    public static class CorsConfig
    {
        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            bool corsEnabled = configuration.GetValue<bool>(ConfigurationConstants.CORS_ENABLED);

            if (corsEnabled)
            {
                services.AddCors();
            }
        }

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            app.UseCors(builder =>
            {
                string[] origins = configuration.GetSection(ConfigurationConstants.CORS_ALLOWED_ORIGINS).GetChildren().Select(x => x.Value).ToArray();
                string[] headers = configuration.GetSection(ConfigurationConstants.CORS_ALLOWED_HEADERS).GetChildren().Select(x => x.Value).ToArray();
                string[] exposedHeaders = configuration.GetSection(ConfigurationConstants.CORS_EXPOSED_HEADERS).GetChildren().Select(x => x.Value).ToArray();
                string[] allowedMethods = configuration.GetSection(ConfigurationConstants.CORS_ALLOWED_METHODS).GetChildren().Select(x => x.Value).ToArray();
                bool allowCredentials = configuration.GetValue<bool>(ConfigurationConstants.CORS_ALLOW_CREDENTIALS);

                if (origins.Any())
                {
                    builder.WithOrigins(origins);
                }
                else
                {
                    builder.AllowAnyOrigin();
                }

                if (headers.Any())
                {
                    builder.WithHeaders(headers);
                }
                else
                {
                    builder.AllowAnyHeader();
                }

                if (allowedMethods.Any())
                {
                    builder.WithMethods(allowedMethods);
                }
                else
                {
                    builder.AllowAnyMethod();
                }

                if (exposedHeaders.Any())
                {
                    builder.WithExposedHeaders(exposedHeaders);
                }

                if (allowCredentials)
                {
                    builder.AllowCredentials();
                }
            });
        }
    }
}
