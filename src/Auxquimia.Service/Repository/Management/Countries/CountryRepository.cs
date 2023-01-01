namespace Auxquimia.Repository.Management.Countries
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    internal class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }
        
        /// <summary>
        /// Finds the by iso name asynchronous.
        /// </summary>
        /// <param name="isoname">The iso name.</param>
        /// <returns></returns>
        public Task<Country> FindByIsoNameAsync(string isoName)
        {
            return _session.QueryOver<Country>().Where(x => x.IsoName == isoName).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Task<Country> FindByNameAsync(string name)
        {
            return _session.QueryOver<Country>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override Task<Country> GetAsync(Guid id)
        {
            Task<Country> result = _session.QueryOver<Country>().Where(x => x.Id == id).SingleOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Paginateds the asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Task<IList<Country>> SearchByFilter(FindRequestImpl<CountrySearchFilter> filter)
        {
            IQueryOver<Country, Country> qo = _session.QueryOver<Country>();

            if (filter != null && filter.Filter != null)
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

            return qo.ListAsync();
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
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override async Task<Country> UpdateAsync(Country entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        public override Task<IList<Country>> GetAllAsync()
        {
            return GetAllAsync();
        }
    }
}
