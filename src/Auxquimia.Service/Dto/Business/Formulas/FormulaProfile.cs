namespace Auxquimia.Dto.Business.Formulas
{
    using AutoMapper;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Management.Metrics;
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Repository.Business.Formulas;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="FormulaProfile" />.
    /// </summary>
    public class FormulaProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaProfile"/> class.
        /// </summary>
        public FormulaProfile()
        {
            // Formula search filter
            CreateMap<FindRequestImpl<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();


            // Formula
            CreateMap<Formula, FormulaDto>()
                .ForMember(x => x.Steps, opt => opt.MapFrom<DetachedStepsResolver>());
            CreateMap<FormulaDto, Formula>()
                 .ForMember(x => x.Steps, opt => opt.Ignore())
                 .ForMember(x => x.AssemblyBuild, opt => opt.MapFrom<AssemblyBuildResolver, string>(y => y.AssemblyBuild != null ? y.AssemblyBuild.Id : null));
            CreateMap<Page<Formula>, Page<FormulaDto>>();

            // Formula Step
            CreateMap<FormulaStep, FormulaStepDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(y => y.Operator != null ? y.Operator.Username : null));
            CreateMap<FormulaStepDto, FormulaStep>()
                .ForMember(x => x.Operator, opt => opt.MapFrom<UserNameResolver, string>(y => y.Username != null ? y.Username : null))
                .ForMember(x => x.Operator, opt => opt.MapFrom<UserNameResolver, string>(y => y.Operator != null ? y.Operator.Username : null))
                .ForMember(x => x.Formula, opt => opt.MapFrom<FormulaResolver, string>(y => y.Formula != null ? y.Formula.Id : null));
            CreateMap<Page<FormulaStep>, Page<FormulaStepDto>>();


            //Netsuite Formula
            CreateMap<NetsuiteFormula, NetsuiteFormulaDto>()
                .ForMember(x => x.Steps, opt => opt.MapFrom<DetachedNetsuiteStepsResolver>());
            CreateMap<NetsuiteFormulaDto, NetsuiteFormula>()
                 .ForMember(x => x.Steps, opt => opt.Ignore())
                 .ForMember(x => x.Units, opt => opt.MapFrom<UnitResolver, string>(y => y.Units != null ? y.Units.Id : null))
                 .ForMember(x => x.AssemblyBuild, opt => opt.MapFrom<AssemblyBuildResolver, string>(y => y.AssemblyBuild != null ? y.AssemblyBuild.Id : null));
            CreateMap<Page<NetsuiteFormula>, Page<NetsuiteFormulaDto>>();

            //Netsuite Formula NetsuiteStep
            CreateMap<NetsuiteFormulaStep, NetsuiteFormulaStepDto>();
            CreateMap<NetsuiteFormulaStepDto, NetsuiteFormulaStep>()
                 .ForMember(x => x.Operator, opt => opt.MapFrom<UserNameResolver, string>(y => y.Operator != null ? y.Operator.Username : null))
                 .ForMember(x => x.Units, opt => opt.MapFrom<UnitResolver, string>(y => y.Units != null ? y.Units.Id : null))
                 .ForMember(x => x.Formula, opt => opt.MapFrom<NetsuiteFormulaResolver, string>(y => y.Formula != null ? y.Formula.Id : null));
            CreateMap<Page<NetsuiteFormulaStep>, Page<NetsuiteFormulaStepDto>>();

        }
    }

    /// <summary>
    /// Defines the <see cref="FormulaResolver" />.
    /// </summary>
    internal class FormulaResolver : IMemberValueResolver<object, object, string, Formula>
    {
        /// <summary>
        /// Defines the formulaRepository.
        /// </summary>
        private readonly IFormulaRepository formulaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaResolver"/> class.
        /// </summary>
        /// <param name="formulaRepository">The formulaRepository<see cref="IFormulaRepository"/>.</param>
        public FormulaResolver(IFormulaRepository formulaRepository)
        {
            this.formulaRepository = formulaRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="Formula"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="Formula"/>.</returns>
        public Formula Resolve(object source, object destination, string sourceMember, Formula destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.formulaRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaResolver" />.
    /// </summary>
    internal class NetsuiteFormulaResolver : IMemberValueResolver<object, object, string, NetsuiteFormula>
    {
        /// <summary>
        /// Defines the formulaRepository.
        /// </summary>
        private readonly INetsuiteFormulaRepository formulaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaResolver"/> class.
        /// </summary>
        /// <param name="formulaRepository">The formulaRepository<see cref="INetsuiteFormulaRepository"/>.</param>
        public NetsuiteFormulaResolver(INetsuiteFormulaRepository formulaRepository)
        {
            this.formulaRepository = formulaRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="NetsuiteFormula"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="NetsuiteFormula"/>.</returns>
        public NetsuiteFormula Resolve(object source, object destination, string sourceMember, NetsuiteFormula destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.formulaRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStepResolver" />.
    /// </summary>
    internal class NetsuiteFormulaStepResolver : IMemberValueResolver<object, object, string, NetsuiteFormulaStep>
    {
        /// <summary>
        /// Defines the formulaStepRepository.
        /// </summary>
        private readonly INetsuiteFormulaStepRepository formulaStepRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaStepResolver"/> class.
        /// </summary>
        /// <param name="formulaStepRepository">The formulaStepRepository<see cref="INetsuiteFormulaStepRepository"/>.</param>
        public NetsuiteFormulaStepResolver(INetsuiteFormulaStepRepository formulaStepRepository)
        {
            this.formulaStepRepository = formulaStepRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="NetsuiteFormulaStep"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="NetsuiteFormulaStep"/>.</returns>
        public NetsuiteFormulaStep Resolve(object source, object destination, string sourceMember, NetsuiteFormulaStep destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.formulaStepRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    /// <summary>
    /// Defines the <see cref="FormulaStepResolver" />.
    /// </summary>
    internal class FormulaStepResolver : IMemberValueResolver<object, object, string, FormulaStep>
    {
        /// <summary>
        /// Defines the formulaStepRepository.
        /// </summary>
        private readonly IFormulaStepRepository formulaStepRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaStepResolver"/> class.
        /// </summary>
        /// <param name="formulaStepRepository">The formulaStepRepository<see cref="IFormulaStepRepository"/>.</param>
        public FormulaStepResolver(IFormulaStepRepository formulaStepRepository)
        {
            this.formulaStepRepository = formulaStepRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="FormulaStep"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="FormulaStep"/>.</returns>
        public FormulaStep Resolve(object source, object destination, string sourceMember, FormulaStep destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.formulaStepRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    /// <summary>
    /// Defines the <see cref="DetachedStepsResolver" />.
    /// </summary>
    internal class DetachedStepsResolver : IValueResolver<Formula, FormulaDto, IList<FormulaStepDto>>
    {
        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="Formula"/>.</param>
        /// <param name="destination">The destination<see cref="FormulaDto"/>.</param>
        /// <param name="destMember">The destMember<see cref="IList{FormulaStepDto}"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="IList{FormulaStepDto}"/>.</returns>
        public IList<FormulaStepDto> Resolve(Formula source, FormulaDto destination, IList<FormulaStepDto> destMember, ResolutionContext context)
        {

            IList<FormulaStepDto> resultList = new List<FormulaStepDto>();
            Formula formula = new Formula()
            {
                Id = source.Id
            };
            foreach (FormulaStep step in source.Steps)
            {
                FormulaStepDto result = new FormulaStepDto();
                step.Formula = formula;
                result = step.PerformMapping<FormulaStep, FormulaStepDto>();
                resultList.Add(result);
            }
            return resultList.OrderBy((x) => x.Step).ToList();
        }
    }

    /// <summary>
    /// Defines the <see cref="DetachedNetsuiteStepsResolver" />.
    /// </summary>
    internal class DetachedNetsuiteStepsResolver : IValueResolver<NetsuiteFormula, NetsuiteFormulaDto, IList<NetsuiteFormulaStepDto>>
    {
        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="NetsuiteFormula"/>.</param>
        /// <param name="destination">The destination<see cref="NetsuiteFormulaDto"/>.</param>
        /// <param name="destMember">The destMember<see cref="IList{NetsuiteFormulaStepDto}"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="IList{NetsuiteFormulaStepDto}"/>.</returns>
        public IList<NetsuiteFormulaStepDto> Resolve(NetsuiteFormula source, NetsuiteFormulaDto destination, IList<NetsuiteFormulaStepDto> destMember, ResolutionContext context)
        {

            IList<NetsuiteFormulaStepDto> resultList = new List<NetsuiteFormulaStepDto>();
            NetsuiteFormula formula = new NetsuiteFormula()
            {
                Id = source.Id
            };
            foreach (NetsuiteFormulaStep step in source.Steps)
            {
                NetsuiteFormulaStepDto result = new NetsuiteFormulaStepDto();
                step.Formula = formula;
                result = step.PerformMapping<NetsuiteFormulaStep, NetsuiteFormulaStepDto>();
                resultList.Add(result);
            }
            return resultList.OrderBy((x) => x.AdditionSequence).ToList();
        }
    }

}
