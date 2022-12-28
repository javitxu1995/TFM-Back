using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Auxquimia.Batch.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Auxquimia.Batch.xUnit")]
namespace Auxquimia.Batch
{
    public class Application
    {
        private IEnumerable<IRunner> runners;
        private ILogger<Application> logger;
        private IConfiguration configuration;
        private IServiceProvider serviceProvider;

        public Application(IEnumerable<IRunner> runners, ILogger<Application> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.runners = runners;
            this.logger = logger;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
        }

        public async Task<int> Run()
        {
            int result = Constants.EXECUTION_OK;

            logger.LogInformation("Starting up!");

            AllRunner runner = serviceProvider.GetService<AllRunner>();

            try
            {
                await Execute(runner);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error ejecutando runner: {0}", e.Message);
                result = Constants.INTERNAL_ERROR;
            }

            return result;
        }

        private async Task Execute(IRunner runner)
        {
            JobResult result = runner.Run();
            string report = await result.GetReport();
            logger.LogInformation(report);
        }
    }
}
