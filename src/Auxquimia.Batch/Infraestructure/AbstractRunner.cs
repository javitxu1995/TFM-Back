using System;
using System.Collections.Generic;
using System.Linq;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <seealso cref="Auxquimia.Batch.Infraestructure.IRunner" />
    public abstract class AbstractRunner<X, Y> : IRunner
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public JobResult Run()
        {
            PreProcess();

            long read = 0;
            long processed = 0;
            long written = 0;
            long errors = 0;

            long loop = 0;
            IEnumerable<X> readItems = null;

            do
            {
                readItems = GetReader().Read(loop);
                if (readItems != null && readItems.Any())
                {
                    read += readItems.Count();
                    IEnumerable<Y> processedItems = GetProcessor().Process(readItems);

                    if (processedItems != null && processedItems.Any())
                    {
                        processed += processedItems.Count();

                        written += GetWriter().Write(processedItems);
                    }
                }
                loop++;
            } while (readItems != null && readItems.Any());

            PostProcess();

            return GetJobResult(read, processed, written, errors);
        }

        /// <summary>
        /// Gets the job result.
        /// </summary>
        /// <param name="read">The read.</param>
        /// <param name="processed">The processed.</param>
        /// <param name="written">The written.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        internal abstract JobResult GetJobResult(long read, long processed, long written, long errors);

        /// <summary>
        /// Posts the process.
        /// </summary>
        internal abstract void PostProcess();

        /// <summary>
        /// Gets the writer.
        /// </summary>
        /// <returns></returns>
        internal abstract IWriter<Y> GetWriter();

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <returns></returns>
        internal abstract IProcessor<X, Y> GetProcessor();

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <returns></returns>
        internal abstract IReader<X> GetReader();

        /// <summary>
        /// Pres the process.
        /// </summary>
        internal abstract void PreProcess();
    }
}
