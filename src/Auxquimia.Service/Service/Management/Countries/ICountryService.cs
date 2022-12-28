namespace Auxquimia.Service.Management.Countries
{
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;
    /// <summary>
    /// Service to handle Country entity related operations
    /// </summary>
    public interface ICountryService : IService<CountryDto, Guid>, ISupportsSave<CountryDto, Guid>, ISearchableService<CountryDto, CountrySearchFilter>
    {


        /// <summary>
        /// Finds the by iso name asynchronous
        /// </summary>
        /// <param name="isoname"></param>
        /// <returns></returns>
        Task<CountryDto> FindByIsoNameAsync(string isoName);

        /// <summary>
        /// Find the by name asynchronous
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<CountryDto> FindByNameAsync(string name);
    }
}
