namespace Auxquimia.Dto.Management.Factories
{
    using AutoMapper;
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FactoryProfile" />.
    /// </summary>
    public class FactoryProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryProfile"/> class.
        /// </summary>
        public FactoryProfile()
        {

            // Base search filter
            CreateMap<FindRequestDto<BaseSearchFilter>, FindRequestDto<BaseSearchFilter>>();

            // Reactor
            CreateMap<Reactor, ReactorDto>();
            CreateMap<ReactorDto, Reactor>();
            CreateMap<Page<Reactor>, Page<ReactorDto>>();



            // Factory
            CreateMap<Factory, FactoryDto>()
                .ForMember(x => x.FactoryManagers, opt => opt.MapFrom(y => y.FactoryManagers.Select(x => x.Manager)));
            CreateMap<FactoryDto, Factory>()
                .ForMember(x => x.Country, opt => opt.MapFrom<CountryResolver, string>(y => y.Country != null ? y.Country.Id : null))
                .ForMember(x => x.Reactors, opt => opt.Ignore())
                .ForMember(x => x.FactoryManagers, opt => opt.Ignore());
            CreateMap<Page<Factory>, Page<FactoryDto>>();
            // Factory List 
            CreateMap<Factory, FactoryListDto>()
                .ForMember(x => x.Country, opt => opt.MapFrom(y => y.Country.Name))
                .ForMember(x => x.NoManagers, opt => opt.MapFrom(y => y.FactoryManagers == null || y.FactoryManagers.Count == 0));
            CreateMap<Page<Factory>, Page<FactoryListDto>>();
        }
    }

    /// <summary>
    /// Defines the <see cref="FactoryResolver" />.
    /// </summary>
    internal class FactoryResolver : IMemberValueResolver<object, object, string, Factory>
    {
        /// <summary>
        /// Defines the factoryRepository.
        /// </summary>
        private readonly IFactoryRepository factoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryResolver"/> class.
        /// </summary>
        /// <param name="factoryRepository">The factoryRepository<see cref="IFactoryRepository"/>.</param>
        public FactoryResolver(IFactoryRepository factoryRepository)
        {
            this.factoryRepository = factoryRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="Factory"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="Factory"/>.</returns>
        public Factory Resolve(object source, object destination, string sourceMember, Factory destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.factoryRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    /// <summary>
    /// Defines the <see cref="ReactorResolver" />.
    /// </summary>
    internal class ReactorResolver : IMemberValueResolver<object, object, string, Reactor>
    {
        /// <summary>
        /// Defines the reactorRepository.
        /// </summary>
        private readonly IReactorRepository reactorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorResolver"/> class.
        /// </summary>
        /// <param name="reactorRepository">The reactorRepository<see cref="IReactorRepository"/>.</param>
        public ReactorResolver(IReactorRepository reactorRepository)
        {
            this.reactorRepository = reactorRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="Reactor"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="Reactor"/>.</returns>
        public Reactor Resolve(object source, object destination, string sourceMember, Reactor destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.reactorRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }
}
