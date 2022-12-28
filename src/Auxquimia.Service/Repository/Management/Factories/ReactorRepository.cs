namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReactorRepository" />.
    /// </summary>
    internal class ReactorRepository : NHibernateRepository, IReactorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public ReactorRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The FindByCodeAsync.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public Task<Reactor> FindByCodeAsync(string code)
        {
            return CurrentSession.QueryOver<Reactor>().Where(x => x.Code == code).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public Task<Reactor> FindByNameAsync(string name)
        {
            return CurrentSession.QueryOver<Reactor>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Reactor}}"/>.</returns>
        public Task<IList<Reactor>> GetAllAsync()
        {
            return GetAllAsync<Reactor>();
        }

        public Task<IList<Reactor>> GetAllAsyncWithSession(ISession session)
        {
            return session.QueryOver<Reactor>().ListAsync();
        }

        /// <summary>
        /// The GetAllSync.
        /// </summary>
        /// <returns>The <see cref="IList{Reactor}"/>.</returns>
        public IList<Reactor> GetAllSync()
        {
            return CurrentSession.QueryOver<Reactor>().List();

            //return SessionFactory.GetCurrentSession().QueryOver<Reactor>().List();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public Task<Reactor> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<Reactor>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{Reactor}}"/>.</returns>
        public Task<Page<Reactor>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Reactor>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{ReactorSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Reactor}}"/>.</returns>
        public Task<Page<Reactor>> PaginatedAsync(FindRequestImpl<ReactorSearchFilter> filter)
        {
            IQueryOver<Reactor, Reactor> qo = CurrentSession.QueryOver<Reactor>();

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

            return PaginatedAsync(qo, filter.PageRequest);
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
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{Reactor}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<Reactor> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Reactor"/>.</param>
        /// <returns>The <see cref="Task{Reactor}"/>.</returns>
        public async Task<Reactor> UpdateAsync(Reactor entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
