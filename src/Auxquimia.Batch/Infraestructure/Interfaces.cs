using System;
using System.Collections.Generic;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    public interface IProcessor<X, Y>
    {
        /// <summary>
        /// Processes the specified elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        IEnumerable<Y> Process(IEnumerable<X> elements);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReader<T>
    {
        /// <summary>
        /// Reads the specified loop.
        /// </summary>
        /// <param name="loop">The loop.</param>
        /// <returns></returns>
        IEnumerable<T> Read(long loop);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriter<T>
    {
        /// <summary>
        /// Writes the specified elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        long Write(IEnumerable<T> elements);
    }
}
