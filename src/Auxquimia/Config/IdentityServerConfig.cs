namespace Auxquimia
{
    using IdentityModel;
    using IdentityServer4.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Auxquimia.Security;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Defines the <see cref="IdentityServerConfig" />
    /// </summary>
    public static class IdentityServerConfig
    {
        /// <summary>
        /// The GetApiResources
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <returns>The <see cref="IEnumerable{ApiResource}"/></returns>
        private static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(ConfigurationConstants.IDENTITY_API_RESOURCES);
            List<ApiResource> resources = new List<ApiResource>();

            foreach (IConfigurationSection region in section.GetChildren())
            {
                resources.Add(new ApiResource
                {
                    Name = region.GetValue<string>("name"),
                    DisplayName = region.GetValue<string>("display_name")
                });
            }

            return resources;
        }

        /// <summary>
        /// The GetClients
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        /// <returns>The <see cref="IEnumerable{Client}"/></returns>
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection(ConfigurationConstants.IDENTITY_API_CLIENTS);
            List<Client> clients = new List<Client>();

            foreach (IConfigurationSection region in section.GetChildren())
            {
                clients.Add(new Client
                {
                    ClientId = region.GetValue<string>("client_id"),
                    ClientSecrets = region.GetSection("client_secrets")?.GetChildren()?.Select(x => new Secret(x.Value.Sha256())).ToArray(),
                    AllowedGrantTypes = region.GetSection("allowed_grant_types")?.GetChildren()?.Select(x => x.Value).ToArray(),
                    AllowOfflineAccess = region.GetValue("allow_offline_access", false),
                    AllowedScopes = region.GetSection("allowed_scopes")?.GetChildren()?.Select(x => x.Value).ToArray(),
                    AccessTokenLifetime = region.GetValue("access_token_lifetime", 3600),
                    AbsoluteRefreshTokenLifetime = region.GetValue("refresh_token_lifetime", 2592000),
                    SlidingRefreshTokenLifetime = region.GetValue("sliding_refresh_token_lifetime", 1296000)
                });
            }

            return clients;
        }

        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // configure identity server with in-memory stores, keys, clients and resources
            IIdentityServerBuilder identityServerBuilder = services.AddIdentityServer();

            if (configuration.GetValue<bool>(ConfigurationConstants.IDENTITY_REDIS_ENABLED))
            {
                identityServerBuilder
                    .AddOperationalStore(options =>
                    {
                        options.RedisConnectionString = configuration.GetValue<string>(ConfigurationConstants.IDENTITY_REDIS_CONNECTION_STRING);
                        options.KeyPrefix = configuration.GetValue<string>(ConfigurationConstants.IDENTITY_CREDENTIAL_REDIS_DB_PREFIX);
                        options.Db = configuration.GetValue<int>(ConfigurationConstants.IDENTITY_CREDENTIAL_REDIS_DB_NUM);
                    })
                    .AddPersistedGrantStore<IdentityServer4.Contrib.RedisStore.Stores.PersistedGrantStore>();
            } else
            {
                identityServerBuilder
                    .AddInMemoryPersistedGrants();
            }

            if (configuration.GetValue<bool>(ConfigurationConstants.IDENTITY_USE_DEVELOPER_CREDENTIAL))
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                string path = configuration.GetValue<string>(ConfigurationConstants.IDENTITY_CREDENTIAL_PATH);
                string password = configuration.GetValue<string>(ConfigurationConstants.IDENTITY_CREDENTIAL_PASSWORD);
                identityServerBuilder.AddSigningCredential(new X509Certificate2(path, password));
            }

            identityServerBuilder
                .AddInMemoryApiResources(GetApiResources(configuration))
                .AddInMemoryClients(GetClients(configuration))
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddExtensionGrantValidator<ProviderGrantValidator>()
                .AddProfileService<ProfileService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("isAdministrator", policyAdmin => policyAdmin.RequireClaim(JwtClaimTypes.Role, "ADMINISTRATOR"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddLocalAccessTokenValidation(JwtBearerDefaults.AuthenticationScheme, null, null);
        }

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/></param>
        /// <param name="env">The env<see cref="IHostingEnvironment"/></param>
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIdentityServer();
            app.UseAuthentication();
        }
    }
}
