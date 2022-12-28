namespace Auxquimia.Dto.Management.Countries
{
    using AutoMapper;
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Repository.Management.Countries;
    using Auxquimia.Service.Filters.Management.Countries;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;

    /// <summary>
    /// Defines the <see cref="CountryProfile" />.
    /// </summary>
    internal class CountryProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryProfile"/> class.
        /// </summary>
        public CountryProfile()
        {
            CreateMap<FindRequestDto<CountrySearchFilter>, FindRequestImpl<CountrySearchFilter>>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Page<Country>, Page<CountryDto>>();
        }
    }

    /// <summary>
    /// Defines the <see cref="CountryResolver" />.
    /// </summary>
    internal class CountryResolver : IMemberValueResolver<object, object, string, Country>
    {
        /// <summary>
        /// Defines the countryRepository.
        /// </summary>
        private readonly ICountryRepository countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryResolver"/> class.
        /// </summary>
        /// <param name="countryRepository">The countryRepository<see cref="ICountryRepository"/>.</param>
        public CountryResolver(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="Country"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="Country"/>.</returns>
        public Country Resolve(object source, object destination, string sourceMember, Country destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.countryRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }
}
