namespace Auxquimia.Utils.AutoFac
{
    using Autofac;
    using Autofac.Core.Lifetime;
    using Izertis.Misc.Utils;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AsyncRunner" />.
    /// </summary>
    public class AsyncRunner : IAsyncRunner
    {
        /// <summary>
        /// Gets or sets the LifetimeScope.
        /// </summary>
        private ILifetimeScope LifetimeScope { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRunner"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetimeScope<see cref="ILifetimeScope"/>.</param>
        public AsyncRunner(ILifetimeScope lifetimeScope)
        {
            this.LifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="action">The action<see cref="Action{T}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task Run<T>(Action<T> action)
        {
            Task.Factory.StartNew(() =>
            {
                using (var lifetimeScope = this.LifetimeScope.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
                {
                    //Do something long here
                    var service = lifetimeScope.Resolve<T>();
                    action(service);
                }
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// The RunAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="function">The function<see cref="Func{T, Task}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task RunAsync<T>(Func<T, Task> function)
        {
            Task.Factory.StartNew(() =>
            {
                using (var lifetimeScope = this.LifetimeScope.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
                {
                    //Do something long here
                    var service = lifetimeScope.Resolve<T>();
                    TaskUtils.NonBlockingAwaiter(() => function(service));
                }
            });
            return Task.CompletedTask;
        }
    }
}
