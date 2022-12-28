namespace Auxquimia
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the <see cref="CacheConfig" />
    /// </summary>
    public static class CacheConfig
    {
        /// <summary>
        /// The ConfigureServices
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCacheManager(configuration);
        }
    }
}
