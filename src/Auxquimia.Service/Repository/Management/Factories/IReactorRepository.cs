﻿namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Izertis.Interfaces.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IReactorRepository" />.
    /// </summary>
    public interface IReactorRepository : ISupportsSave<Reactor, Guid>, IDao<Reactor, Guid>, ISearchableDao<Reactor, ReactorSearchFilter>
    {
        /// <summary>
        /// The FindByCodeAsync.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        Task<Reactor> FindByCodeAsync(string code);

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        Task<Reactor> FindByNameAsync(string name);

        /// <summary>
        /// The GetAllSync.
        /// </summary>
        /// <returns>The <see cref="IList{Reactor}"/>.</returns>
        IList<Reactor> GetAllSync();

        /// <summary>
        /// The GetAllAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="IList{Reactor}"/>.</returns>
        Task<IList<Reactor>> GetAllAsyncWithSession(ISession session);
    }
}
