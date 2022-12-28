namespace Auxquimia.Repository.Management.Countries
{
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    internal class CountryRepository : NHibernateRepository, ICountryRepository
    {
        public CountryRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }
        
        /// <summary>
        /// Finds the by iso name asynchronous.
        /// </summary>
        /// <param name="isoname">The iso name.</param>
        /// <returns></returns>
        public Task<Country> FindByIsoNameAsync(string isoName)
        {
            return CurrentSession.QueryOver<Country>().Where(x => x.IsoName == isoName).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Task<Country> FindByNameAsync(string name)
        {
            return CurrentSession.QueryOver<Country>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task<IList<Country>> GetAllAsync()
        {
            return GetAllAsync<Country>();
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task<Country> GetAsync(Guid id)
        {
            Task<Country> result = CurrentSession.QueryOver<Country>().Where(x => x.Id == id).SingleOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Paginateds the asynchronous.
        /// </summary>
        /// <param name="pageRequest">The page request.</param>
        /// <returns></returns>
        public Task<Page<Country>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Country>(), pageRequest);
        }

        /// <summary>
        /// Paginateds the asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Task<Page<Country>> PaginatedAsync(FindRequestImpl<CountrySearchFilter> filter)
        {
            IQueryOver<Country, Country> qo = CurrentSession.QueryOver<Country>();

            if (filter.Filter != null)
            {
                CountrySearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Country>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

                if (StringUtils.HasText(uFilter.IsoName))
                {
                    qo.And(Restrictions.On<Country>(x => x.IsoName).IsInsensitiveLike(uFilter.IsoName, MatchMode.Anywhere));
                }
            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Country> SaveAsync(Country entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task SaveAsync(IList<Country> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Country> UpdateAsync(Country entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
