namespace Auxquimia.Batch.ScanToProduction
{
    using Auxquimia.Batch.Infraestructure;
    using Auxquimia.Service.Business.Kafka;
    using Izertis.Misc.Utils;

    /// <summary>
    /// Defines the <see cref="ScanToProductionRunner" />.
    /// </summary>
    internal class ScanToProductionRunner : IRunner
    {
        /// <summary>
        /// Defines the auxquimiaKafkaService.
        /// </summary>
        private readonly IAuxquimiaKafkaService auxquimiaKafkaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToProductionRunner"/> class.
        /// </summary>
        /// <param name="auxquimiaKafkaService">The auxquimiaKafkaService<see cref="IAuxquimiaKafkaService"/>.</param>
        public ScanToProductionRunner(IAuxquimiaKafkaService auxquimiaKafkaService)
        {
            this.auxquimiaKafkaService = auxquimiaKafkaService;
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <returns>The <see cref="JobResult"/>.</returns>
        public JobResult Run()
        {
            TaskUtils.NonBlockingAwaiter(() => auxquimiaKafkaService.SendAssembliesToProduction());
            return new ScanToProductionJobResult();
        }
    }
}
