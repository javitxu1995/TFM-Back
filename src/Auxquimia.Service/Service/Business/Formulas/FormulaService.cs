namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Config;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Repository.Business.Formulas;
    using Auxquimia.Utils;
    //using Izertis.NHibernate.Repositories;
    //using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FormulaService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class FormulaService : IFormulaService
    {
        /// <summary>
        /// Defines the formulaRepository.
        /// </summary>
        private readonly IFormulaRepository formulaRepository;

        /// <summary>
        /// Defines the formulaStepRepository.
        /// </summary>
        private readonly IFormulaStepRepository formulaStepRepository;

        /// <summary>
        /// Defines the contextConfigProvider.
        /// </summary>
        private readonly ContextConfigProvider contextConfigProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaService"/> class.
        /// </summary>
        /// <param name="formulaRepository">The formulaRepository<see cref="IFormulaRepository"/>.</param>
        /// <param name="formulaStepRepository">The formulaStepRepository<see cref="IFormulaStepRepository"/>.</param>
        /// <param name="contextConfigProvider">The contextConfigProvider<see cref="ContextConfigProvider"/>.</param>
        public FormulaService(IFormulaRepository formulaRepository, IFormulaStepRepository formulaStepRepository, ContextConfigProvider contextConfigProvider)
        {
            this.formulaRepository = formulaRepository;
            this.formulaStepRepository = formulaStepRepository;
            this.contextConfigProvider = contextConfigProvider;
        }

        public async Task<List<FormulaDto>> FindNotOnProduction(FindRequestDto<BaseSearchFilter> filter)
        {
            FindRequestImpl<BaseSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            List<Formula> result = await this.formulaRepository.FindNotOnProduction(findRequest).ConfigureAwait(false);
            //result.Content = result.Content.Where(x => x.AssemblyBuild == null || x.AssemblyBuild.Status == Enums.ABStatus.PENDING).ToList();
            //result.TotalPages = result.Content.Count / result.NumberOfElements;
            return result.PerformMapping<List<Formula>, List<FormulaDto>>();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FormulaDto}}"/>.</returns>
        public async Task<IList<FormulaDto>> GetAllAsync()
        {
            IList<Formula> result = await formulaRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Formula>, IList<FormulaDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaDto}"/>.</returns>
        public async Task<FormulaDto> GetAsync(Guid id)
        {
            Formula result = await formulaRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Formula, FormulaDto>();
        }

        /// <summary>
        /// The GetForAssembly.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        public async Task<Page<FormulaDto>> GetForAssembly(FindRequestDto<BaseSearchFilter> filter)
        {
            FindRequestImpl<BaseSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            Page<Formula> result = await this.formulaRepository.GetForAssembly(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Formula>, Page<FormulaDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        public async Task<Page<FormulaDto>> PaginatedAsync(PageRequest pageRequest)
        {
            Page<Formula> result = await formulaRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Formula>, Page<FormulaDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        public async Task<Page<FormulaDto>> PaginatedAsync(FindRequestDto<BaseSearchFilter> filter)
        {
            FindRequestImpl<BaseSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            Page<Formula> result = await this.formulaRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Formula>, Page<FormulaDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="Task{FormulaDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FormulaDto> SaveAsync(FormulaDto entity)
        {
            Formula formula = entity.PerformMapping<FormulaDto, Formula>();
            Formula result = await formulaRepository.SaveAsync(formula).ConfigureAwait(false);

            // handle steps
            result.Steps = await this.HandleSteps(entity, result).ConfigureAwait(false);

            return result.PerformMapping<Formula, FormulaDto>(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{FormulaDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<FormulaDto> entity)
        {
            return this.formulaRepository.SaveAsync(entity.PerformMapping<IList<FormulaDto>, IList<Formula>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="Task{FormulaDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FormulaDto> UpdateAsync(FormulaDto entity)
        {
            Formula storedFormula = await formulaRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Formula formula = entity.PerformMapping(storedFormula);
            Formula result = await formulaRepository.UpdateAsync(formula).ConfigureAwait(false);

            // handle steps
            result.Steps = await this.HandleSteps(entity, result).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The HandleSteps.
        /// </summary>
        /// <param name="source">The source<see cref="FormulaDto"/>.</param>
        /// <param name="destination">The destination<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<IList<FormulaStep>> HandleSteps(FormulaDto source, Formula destination)
        {
            IList<FormulaStep> steps = source.Steps.PerformMapping<IList<FormulaStepDto>, IList<FormulaStep>>();
            steps = steps ?? new List<FormulaStep>();
            IList<FormulaStep> actualSteps = destination.Steps ?? new List<FormulaStep>();
            // DeleteAsync no longer Steps
            IList<FormulaStep> removedSteps = actualSteps;
            if (removedSteps.Any())
            {
                removedSteps = removedSteps.Except(steps).ToList();
                foreach (FormulaStep step in removedSteps)
                {
                    FormulaStep toDelete = await formulaStepRepository.GetAsync(step.Id).ConfigureAwait(false);
                    if (toDelete != null)
                    {
                        await formulaStepRepository.DeleteAsync(toDelete).ConfigureAwait(false);
                    }
                }
            }

            // Edited steps are managed at the moment so it is not necessary take care about them here

            IList<FormulaStep> newSteps = steps.Where(x => x.Id == default(Guid)).ToList();

            // Add new Steps
            if (newSteps.Any())
            {
                foreach (FormulaStep step in newSteps)
                {
                    step.Formula = destination;
                    // Save on BBDD
                    FormulaStep stepSaved = await formulaStepRepository.SaveAsync(step).ConfigureAwait(false);
                }
            }
            return destination.Steps;
        }
    }
}
