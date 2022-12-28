namespace Auxquimia.Repository.Management.Metrics
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Metrics;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UnitRepository" />.
    /// </summary>
    internal class UnitRepository : NHibernateRepository, IUnitRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public UnitRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Unit}}"/>.</returns>
        public Task<IList<Unit>> GetAllAsync()
        {
            return GetAllAsync<Unit>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public Task<Unit> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<Unit>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{Unit}}"/>.</returns>
        public Task<Page<Unit>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Unit>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Unit}}"/>.</returns>
        public Task<Page<Unit>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Unit, Unit> qo = CurrentSession.QueryOver<Unit>();

            if (filter.Filter != null)
            {
                BaseSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Code))
                {
                    qo.And(Restrictions.On<Unit>(x => x.Code).IsInsensitiveLike(uFilter.Code, MatchMode.Anywhere));
                }
                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Unit>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Unit"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public async Task<Unit> SaveAsync(Unit entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{Unit}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<Unit> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        public Task<Unit> FindByCode(string code)
        {
            return CurrentSession.QueryOver<Unit>().Where(x => x.Code == code).SingleOrDefaultAsync();
        }

        public Task<Unit> FindByName(string name)
        {
            return CurrentSession.QueryOver<Unit>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Unit"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public async Task<Unit> UpdateAsync(Unit entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
