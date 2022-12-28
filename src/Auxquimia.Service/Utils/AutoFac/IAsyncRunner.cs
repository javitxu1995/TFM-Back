namespace Auxquimia.Utils.AutoFac
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAsyncRunner" />.
    /// </summary>
    public interface IAsyncRunner
    {
        /// <summary>
        /// The Run.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="action">The action<see cref="Action{T}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Run<T>(Action<T> action);

        /// <summary>
        /// The RunAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="function">The function<see cref="Func{T, Task}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RunAsync<T>(Func<T, Task> function);
    }
}
