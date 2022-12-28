namespace Auxquimia.Batch.NetsuiteRead
{
    using Auxquimia.Batch.Infraestructure;
    using Auxquimia.Service.Business.AssemblyBuilds;
    using Izertis.Misc.Utils;

    /// <summary>
    /// Defines the <see cref="NetsuiteReadRunner" />.
    /// </summary>
    internal class NetsuiteReadRunner : IRunner
    {
        /// <summary>
        /// Defines the assemblyBuildService.
        /// </summary>
        private readonly IAssemblyBuildService assemblyBuildService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteReadRunner"/> class.
        /// </summary>
        /// <param name="assemblyBuildService">The assemblyBuildService<see cref="IAssemblyBuildService"/>.</param>
        public NetsuiteReadRunner(IAssemblyBuildService assemblyBuildService)
        {
            this.assemblyBuildService = assemblyBuildService;
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <returns>The <see cref="JobResult"/>.</returns>
        public JobResult Run()
        {
            TaskUtils.NonBlockingAwaiter(() => assemblyBuildService.LoadFromFtp());
            return new NetsuiteReadJobResult();
        }
    }
}
