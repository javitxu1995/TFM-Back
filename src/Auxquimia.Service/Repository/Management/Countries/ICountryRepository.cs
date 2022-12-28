namespace Auxquimia.Repository.Management.Countries
{
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    interface ICountryRepository : ISupportsSave<Country, Guid>, IDao<Country, Guid>, ISearchableDao<Country, CountrySearchFilter>
    {
        /// <summary>
        /// Finds the by iso name asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<Country> FindByIsoNameAsync(string isoName);

        /// <summary>
        /// Finds the by country name asynchronous.
        /// </summary>
        /// <param name="name">The country name.</param>
        /// <returns></returns>
        Task<Country> FindByNameAsync(string name);
    }
}
