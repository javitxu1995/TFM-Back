namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReactorRepository" />.
    /// </summary>
    internal class ReactorRepository : RepositoryBase<Reactor>, IReactorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public ReactorRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The FindByCodeAsync.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public Task<Reactor> FindByCodeAsync(string code)
        {
            return _session.QueryOver<Reactor>().Where(x => x.Code == code).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public Task<Reactor> FindByNameAsync(string name)
        {
            return _session.QueryOver<Reactor>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Reactor}}"/>.</returns>
        public override Task<IList<Reactor>> GetAllAsync()
        {
            return _session.QueryOver<Reactor>().ListAsync();
        }

        /// <summary>
        /// The GetAllSync.
        /// </summary>
        /// <returns>The <see cref="IList{Reactor}"/>.</returns>
        public IList<Reactor> GetAllSync()
        {
            return _session.QueryOver<Reactor>().List();
        }

        public Task<IList<Reactor>> GetAllAsyncWithSession(ISession session)
        {
            return session.QueryOver<Reactor>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public override Task<Reactor> GetAsync(Guid id)
        {
            return _session.QueryOver<Reactor>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{ReactorSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Reactor}}"/>.</returns>
        public Task<IList<Reactor>> SearchByFilter(FindRequestImpl<ReactorSearchFilter> filter)
        {
            IQueryOver<Reactor, Reactor> qo = _session.QueryOver<Reactor>();

            if (filter.Filter != null)
            {
                ReactorSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Reactor>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

                if (StringUtils.HasText(uFilter.Code))
                {
                    qo.And(Restrictions.On<Reactor>(x => x.Code).IsInsensitiveLike(uFilter.Code, MatchMode.Anywhere));
                }
                if (uFilter.FactoryId != default(Guid))
                {
                    qo.And(x => x.Factory.Id == uFilter.FactoryId);
                }
            }

            return qo.ListAsync();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Reactor"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public async Task<Reactor> SaveAsync(Reactor entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Reactor"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public async override Task<Reactor> UpdateAsync(Reactor entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
