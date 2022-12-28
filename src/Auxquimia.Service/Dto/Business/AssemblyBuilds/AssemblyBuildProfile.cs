namespace Auxquimia.Dto.Business.AssemblyBuilds
{
    using AutoMapper;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Repository.Management.Business.AssemblyBuilds;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using static Auxquimia.Utils.Constants;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildProfile" />.
    /// </summary>
    public class AssemblyBuildProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildProfile"/> class.
        /// </summary>
        public AssemblyBuildProfile()
        {
            // Assembly Build search filter
            CreateMap<FindRequestDto<BaseAssemblyBuildSearchFilter>, FindRequestImpl<BaseAssemblyBuildSearchFilter>>();


            // Assembly Build
            CreateMap<AssemblyBuild, AssemblyBuildDto>()
            .ForMember(x => x.NetsuiteFormula, opt => opt.MapFrom<DetachedAssemblyNetsuiteFormulaResolver>())
            .ForMember(x => x.Formula, opt => opt.MapFrom<DetachedAssemblyFormulaResolver>());
            CreateMap<AssemblyBuildDto, AssemblyBuild>()
                .ForMember(x => x.Operator, opt => opt.MapFrom<UserResolver, string>(y => y.Operator != null ? y.Operator.Id : null))
                .ForMember(x => x.Factory, opt => opt.MapFrom<FactoryResolver, string>(y => y.Factory != null ? y.Factory.Id : null))
                .ForMember(x => x.Formula, opt => opt.MapFrom<FormulaResolver, string>(y => y.Formula != null ? y.Formula.Id : null))
                .ForMember(x => x.Blender, opt => opt.MapFrom<ReactorResolver, string>(y => y.Blender != null ? y.Blender.Id : null));
            CreateMap<Page<AssemblyBuild>, Page<AssemblyBuildDto>>();

            //Assembly Build List
            CreateMap<AssemblyBuild, AssemblyBuildListDto>()
                .ForMember(x => x.Blender, opt => opt.MapFrom(y => y.Blender != null ? y.Blender.Code : string.Empty))
                .ForMember(x => x.BlenderName, opt => opt.MapFrom(y => y.Blender != null ? y.Blender.Name : string.Empty))
                .ForMember(x => x.FactoryName, opt => opt.MapFrom(y => y.Factory != null ? y.Factory.Name : string.Empty))
                .ForMember(x => x.FormulaName, opt => opt.MapFrom<DetachedAssemblyListFormulaNameResolver>());
            CreateMap<Page<AssemblyBuild>, Page<AssemblyBuildListDto>>();
        }
    }

    /// <summary>
    /// Defines the <see cref="AssemblyBuildResolver" />.
    /// </summary>
    internal class AssemblyBuildResolver : IMemberValueResolver<object, object, string, AssemblyBuild>
    {
        /// <summary>
        /// Defines the assemblyBuildRepository.
        /// </summary>
        private readonly IAssemblyBuildRepository assemblyBuildRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildResolver"/> class.
        /// </summary>
        /// <param name="assemblyBuildRepository">The assemblyBuildRepository<see cref="IAssemblyBuildRepository"/>.</param>
        public AssemblyBuildResolver(IAssemblyBuildRepository assemblyBuildRepository)
        {
            this.assemblyBuildRepository = assemblyBuildRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="AssemblyBuild"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="AssemblyBuild"/>.</returns>
        public AssemblyBuild Resolve(object source, object destination, string sourceMember, AssemblyBuild destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.assemblyBuildRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    public class DetachedAssemblyNetsuiteFormulaResolver : IValueResolver<AssemblyBuild, AssemblyBuildDto, NetsuiteFormulaDto>
    {

        public NetsuiteFormulaDto Resolve(AssemblyBuild source, AssemblyBuildDto destination, NetsuiteFormulaDto destMember, ResolutionContext context)
        {
            NetsuiteFormulaDto formula = new NetsuiteFormulaDto();
            if(source.NetsuiteFormula != null)
            {
                source.NetsuiteFormula.AssemblyBuild = new AssemblyBuild()
                {
                    Id = source.Id,
                    Date = source.Date,
                    AssemblyBuildNumber = source.AssemblyBuildNumber,
                    Blender = new Reactor()
                    {
                        Id = source.Blender.Id,
                        Name = source.Blender.Name,
                        Code = source.Blender.Code
                    }
                };
            }
            formula = source.NetsuiteFormula.PerformMapping<NetsuiteFormula, NetsuiteFormulaDto>();
            return formula;
        }
    }

    public class DetachedAssemblyFormulaResolver : IValueResolver<AssemblyBuild, AssemblyBuildDto, FormulaDto>
    {

        public FormulaDto Resolve(AssemblyBuild source, AssemblyBuildDto destination, FormulaDto destMember, ResolutionContext context)
        {
            FormulaDto formula = new FormulaDto();
            if (source.Formula != null)
            {
                source.Formula.AssemblyBuild = new AssemblyBuild()
                {
                    Id = source.Id,
                    Date = source.Date,
                    AssemblyBuildNumber = source.AssemblyBuildNumber,
                    Blender = new Reactor()
                    {
                        Id = source.Blender.Id,
                        Name = source.Blender.Name,
                        Code = source.Blender.Code
                    }
                };
            }
            formula = source.Formula.PerformMapping<Formula, FormulaDto>();
            return formula;
        }
    }

    public class DetachedAssemblyReactorResolver : IValueResolver<AssemblyBuild, AssemblyBuildListDto, string>
    {

        public string Resolve(AssemblyBuild source, AssemblyBuildListDto destination, string destMember, ResolutionContext context)
        {

            if (source.Blender != null)
            {
                string code = source.Blender.Code;
                return code;
            }
            return string.Empty;
        }
    }

    public class DetachedAssemblyListFormulaNameResolver : IValueResolver<AssemblyBuild, AssemblyBuildListDto, string>
    {

        public string Resolve(AssemblyBuild source, AssemblyBuildListDto destination, string destMember, ResolutionContext context)
        {
            string name = "";
            if (source.Formula != null)
            {
                name = source.Formula.Name;
            }
            else if(source.NetsuiteFormula != null)
            {
                name = source.NetsuiteFormula.Name;
            }
            return name;
        }
    }
}
