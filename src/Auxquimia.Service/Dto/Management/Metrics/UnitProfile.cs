namespace Auxquimia.Dto.Management.Metrics
{
    using AutoMapper;
    using Auxquimia.Model.Management.Metrics;
    using Auxquimia.Repository.Management.Metrics;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;

    /// <summary>
    /// Defines the <see cref="UnitProfile" />.
    /// </summary>
    public class UnitProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitProfile"/> class.
        /// </summary>
        public UnitProfile()
        {
            //Unit
            CreateMap<Unit, UnitDto>();
            CreateMap<UnitDto, Unit>();

            CreateMap<Page<Unit>, Page<UnitDto>>();
        }
    }

    /// <summary>
    /// Defines the <see cref="UnitResolver" />.
    /// </summary>
    internal class UnitResolver : IMemberValueResolver<object, object, string, Unit>
    {
        /// <summary>
        /// Defines the unitRepository.
        /// </summary>
        private readonly IUnitRepository unitRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitResolver"/> class.
        /// </summary>
        /// <param name="unitRepository">The unitRepository<see cref="IUnitRepository"/>.</param>
        public UnitResolver(IUnitRepository unitRepository)
        {
            this.unitRepository = unitRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="Unit"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="Unit"/>.</returns>
        public Unit Resolve(object source, object destination, string sourceMember, Unit destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.unitRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }
}
