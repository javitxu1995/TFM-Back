using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Auxquimia.Batch.Infraestructure.IRunner" />
    public class AllRunner : IRunner
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILogger<AllRunner> logger;

        private readonly IServiceProvider serviceProvider;

        public AllRunner(ILogger<AllRunner> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public JobResult Run()
        {
            logger.LogInformation("Iniciando todos los procesos");
            Stopwatch.GetInstance().RegisterEvent("Started all processes");
            JobResultAggregator results = new JobResultAggregator();

            IEnumerable<IRunner> runners = serviceProvider.GetServices<IRunner>();

            IList<IRunner> otherRunners = runners.Where(x => x.GetType() != typeof(AllRunner)).ToList();

            foreach (IRunner runner in otherRunners)
            {
                logger.LogInformation("Running: {0}", runner.GetType().Name);

                try
                {
                    results.Append(runner.Run());
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Job {0} failed with message {1}", runner.GetType().Name, e.Message);
                    results.Append(new FailedJobResult()
                    {
                        Heading = runner.GetType().Name,
                        Exception = e
                    });
                }
            }

            logger.LogInformation("Finalizados todos los procesos");
            return results;
        }
    }
}
