using System;
namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    public interface IRunner
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        JobResult Run();
    }
}
