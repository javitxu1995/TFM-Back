namespace Auxquimia.Repository.Management.Countries
{
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using System;
    using System.Threading.Tasks;

    interface ICountryRepository : IRepositoryBase<Country>, ISupportsSave<Country, Guid>, ISearcheableRepository<Country, CountrySearchFilter>
 
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
