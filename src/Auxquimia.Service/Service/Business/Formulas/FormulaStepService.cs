namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Repository.Business.Formulas;
    using Auxquimia.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FormulaStepService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class FormulaStepService : IFormulaStepService
    {
        /// <summary>
        /// Defines the formulaStepRepository.
        /// </summary>
        private readonly IFormulaStepRepository formulaStepRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaStepService"/> class.
        /// </summary>
        /// <param name="formulaStepRepository">The formulaStepRepository<see cref="IFormulaStepRepository"/>.</param>
        public FormulaStepService(IFormulaStepRepository formulaStepRepository)
        {
            this.formulaStepRepository = formulaStepRepository;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FormulaStepDto}}"/>.</returns>
        public async Task<IList<FormulaStepDto>> GetAllAsync()
        {
            var result = await formulaStepRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<FormulaStep>, IList<FormulaStepDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        public async Task<FormulaStepDto> GetAsync(Guid id)
        {
            var result = await formulaStepRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<FormulaStep, FormulaStepDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaStepDto}}"/>.</returns>
        public async Task<Page<FormulaStepDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await formulaStepRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<FormulaStep>, Page<FormulaStepDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaStepDto}}"/>.</returns>
        public async Task<Page<FormulaStepDto>> PaginatedAsync(FindRequestDto<BaseSearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<BaseSearchFilter>, FindRequestDto<BaseSearchFilter>>();
            var result = await formulaStepRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<FormulaStep>, Page<FormulaStepDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FormulaStepDto> SaveAsync(FormulaStepDto entity)
        {
            FormulaStep formula = entity.PerformMapping<FormulaStepDto, FormulaStep>();
            FormulaStep result = await formulaStepRepository.SaveAsync(formula).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{FormulaStepDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<FormulaStepDto> entity)
        {
            return this.formulaStepRepository.SaveAsync(entity.PerformMapping<IList<FormulaStepDto>, IList<FormulaStep>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FormulaStepDto> UpdateAsync(FormulaStepDto entity)
        {
            FormulaStep storedFormulaStep = await formulaStepRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            FormulaStep formulaStep = entity.PerformMapping(storedFormulaStep);
            FormulaStep result = await formulaStepRepository.UpdateAsync(formulaStep).ConfigureAwait(false);


            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The GetNextUnwritedStep.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        public async Task<FormulaStepDto> GetNextUnwritedStep(int step, Guid formulaId)
        {
            IList<FormulaStep> steps = await this.formulaStepRepository.FindStepsFromFormula(formulaId).ConfigureAwait(false);
            steps = steps.Where(s => !s.Written && s.Step == step).ToList();
            if (steps.Count > 0)
            {
                FormulaStepDto stepDto = steps.First().PerformMapping<FormulaStep, FormulaStepDto>();
                return stepDto;
            }
            return null;
        }

        /// <summary>
        /// The MarkStepAsWritted.
        /// </summary>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{FormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FormulaStepDto> MarkStepAsWritted(Guid stepId, ISession session = null)
        {
            FormulaStep step = null;
            if (session != null)
            {
                step = await this.formulaStepRepository.GetAsyncWithSession(session, stepId);
            }
            else
            {
                step = await this.formulaStepRepository.GetAsync(stepId);
            }
            if (step != null)
            {
                step.Written = true;
                if (session != null)
                {
                    step = await this.formulaStepRepository.UpdateStepWithSession(session, step);
                }
                else
                {
                    step = await this.formulaStepRepository.UpdateAsync(step);
                }
                return step.PerformMapping<FormulaStep, FormulaStepDto>();
            }
            return null;
        }
    }
}
