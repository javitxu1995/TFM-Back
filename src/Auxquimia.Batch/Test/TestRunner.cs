namespace Auxquimia.Batch.Test
{
    using Auxquimia.Batch.Infraestructure;

    /// <summary>
    /// Defines the <see cref="TestRunner" />.
    /// </summary>
    internal class TestRunner : IRunner
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns>.</returns>
        public JobResult Run()
        {
            return new TestJobResult();
        }
    }
}
