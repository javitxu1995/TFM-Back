namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
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
    /// Defines the <see cref="FactoryRepository" />.
    /// </summary>
    internal class FactoryRepository : NHibernateRepository, IFactoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FactoryRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The findMainFactory.
        /// </summary>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public Task<Factory> FindMainFactory()
        {
            return CurrentSession.QueryOver<Factory>().Where(x => x.Main).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Factory}}"/>.</returns>
        public Task<IList<Factory>> GetAllAsync()
        {
            return GetAllAsync<Factory>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public Task<Factory> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<Factory>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{Factory}}"/>.</returns>
        public Task<Page<Factory>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Factory>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{FactorySearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Factory}}"/>.</returns>
        public Task<Page<Factory>> PaginatedAsync(FindRequestImpl<FactorySearchFilter> filter)
        {
            IQueryOver<Factory, Factory> qo = CurrentSession.QueryOver<Factory>();

            if (filter.Filter != null)
            {
                FactorySearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Factory>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

                if (StringUtils.HasText(uFilter.Code))
                {
                    qo.And(Restrictions.On<Factory>(x => x.Code).IsInsensitiveLike(uFilter.Code, MatchMode.Anywhere));
                }
                if (uFilter.CountryId != null && !uFilter.CountryId.Equals(default(Guid)))
                {
                    qo.And(x => x.Country.Id == uFilter.CountryId);
                }
            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Factory"/>.</param>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public async Task<Factory> SaveAsync(Factory entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{Factory}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<Factory> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Factory"/>.</param>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public async Task<Factory> UpdateAsync(Factory entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
