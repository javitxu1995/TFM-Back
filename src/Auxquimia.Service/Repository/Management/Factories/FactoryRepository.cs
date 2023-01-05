namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters.FindRequests;
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
    /// Defines the <see cref="FactoryRepository" />.
    /// </summary>
    internal class FactoryRepository : RepositoryBase<Factory>, IFactoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FactoryRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The findMainFactory.
        /// </summary>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public Task<Factory> FindMainFactory()
        {
            return _session.QueryOver<Factory>().Where(x => x.Main).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Factory}}"/>.</returns>
        public override Task<IList<Factory>> GetAllAsync()
        {
            return _session.QueryOver<Factory>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public override Task<Factory> GetAsync(Guid id)
        {
            return _session.QueryOver<Factory>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{FactorySearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Factory}}"/>.</returns>
        public Task<IList<Factory>> SearchByFilter(FindRequestImpl<FactorySearchFilter> filter)
        {
            IQueryOver<Factory, Factory> qo = _session.QueryOver<Factory>();

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

            return qo.ListAsync();
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
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Factory"/>.</param>
        /// <returns>The <see cref="Task{Factory}"/>.</returns>
        public async override Task<Factory> UpdateAsync(Factory entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
