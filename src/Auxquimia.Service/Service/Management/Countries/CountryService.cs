namespace Auxquimia.Service.Management.Countries
{
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Repository.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Auxquimia.Utils;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class CountryService : ICountryService
    {

        /// <summary>
        /// Gets or sets the countryRepository
        /// </summary>
        private readonly ICountryRepository countryRepository;

        /// <summary>
        ///  Initializes a new instance of the <see cref="CountryService"/> class.
        /// </summary>
        /// <param name="countryRepository"></param>
        /// <param name="countrySearcher"></param>
        /// <param name="countryIndexer"></param>
        public CountryService(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        /// <summary>
        /// The FindByISONameAsync
        /// </summary>
        /// <param name="name">The username<see cref="string"/></param>
        /// <returns>The <see cref="Task{CountryDto}"/></returns>
        public async Task<CountryDto> FindByIsoNameAsync(string isoName)
        {
            var result = await countryRepository.FindByNameAsync(isoName).ConfigureAwait(false);
            return result.PerformMapping<Country, CountryDto>();
        }

        /// <summary>
        /// The FindByNameAsync
        /// </summary>
        /// <param name="name">The username<see cref="string"/></param>
        /// <returns>The <see cref="Task{CountryDto}"/></returns>
        public async Task<CountryDto> FindByNameAsync(string name)
        {
            var result = await countryRepository.FindByNameAsync(name).ConfigureAwait(false);
            return result.PerformMapping<Country, CountryDto>();
        }

        /// <summary>
        /// The GetAllAsync
        /// </summary>
        /// <returns>The <see cref="Task{IList{CountryDto}}"/></returns>
        public async Task<IList<CountryDto>> GetAllAsync()
        {
            var result = await countryRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Country>, IList<CountryDto>>();
        }

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{UserDto}"/></returns>
        public async Task<CountryDto> GetAsync(Guid id)
        {
            var result = await countryRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Country, CountryDto>();
        }

        /// <summary>
        /// The SearchByFilter
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{CountrySearchFilter}"/></param>
        /// <returns></returns>
        public async Task<IList<CountryDto>> SearchByFilter(FindRequestDto<CountrySearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<CountrySearchFilter>, FindRequestImpl<CountrySearchFilter>>();
            var result = await countryRepository.SearchByFilter(findRequest).ConfigureAwait(false);
            return result.PerformMapping<IList<Country>, IList<CountryDto>>();
        }

        /// <summary>
        /// The SaveAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="CountryDto"/></param>
        /// <returns>The <see cref="Task{CountryDto}"/></returns>
        public async Task<CountryDto> SaveAsync(CountryDto entity)
        {
            Country country = entity.PerformMapping<CountryDto, Country>();
            Country result = await countryRepository.SaveAsync(country).ConfigureAwait(false);
            return result.PerformMapping(entity);
        }
        /// <summary>
        /// The SaveAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{CountryDto}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SaveAsync(IList<CountryDto> entity)
        {
            foreach(CountryDto country in entity)
            {
                await SaveAsync(country).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="CountryDto"/></param>
        /// <returns>The <see cref="Task{CountryDto}"/></returns>
        public async Task<CountryDto> UpdateAsync(CountryDto entity)
        {
            Country storedCountry = await countryRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Country country = entity.PerformMapping(storedCountry);
            Country result = await countryRepository.UpdateAsync(country).ConfigureAwait(false);
            return result.PerformMapping(entity);
        }
    }
}
