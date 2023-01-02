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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class NetsuiteFormulaService : INetsuiteFormulaService
    {
        /// <summary>
        /// Defines the netsuiteFormulaRepository.
        /// </summary>
        private readonly INetsuiteFormulaRepository netsuiteFormulaRepository;

        /// <summary>
        /// Defines the netsuiteFormulaStepRepository.
        /// </summary>
        private readonly INetsuiteFormulaStepRepository netsuiteFormulaStepRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaService"/> class.
        /// </summary>
        /// <param name="netsuiteFormulaRepository">The netsuiteFormulaRepository<see cref="INetsuiteFormulaRepository"/>.</param>
        /// <param name="netsuiteFormulaStepRepository">The netsuiteFormulaStepRepository<see cref="INetsuiteFormulaStepRepository"/>.</param>
        public NetsuiteFormulaService(INetsuiteFormulaRepository netsuiteFormulaRepository, INetsuiteFormulaStepRepository netsuiteFormulaStepRepository)
        {
            this.netsuiteFormulaRepository = netsuiteFormulaRepository;
            this.netsuiteFormulaStepRepository = netsuiteFormulaStepRepository;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaDto}}"/>.</returns>
        public async Task<IList<NetsuiteFormulaDto>> GetAllAsync()
        {
            IList<NetsuiteFormula> result = await netsuiteFormulaRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<NetsuiteFormula>, IList<NetsuiteFormulaDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaDto}"/>.</returns>
        public async Task<NetsuiteFormulaDto> GetAsync(Guid id)
        {
            NetsuiteFormula result = await netsuiteFormulaRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<NetsuiteFormula, NetsuiteFormulaDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormulaDto}}"/>.</returns>
        public async Task<Page<NetsuiteFormulaDto>> PaginatedAsync(PageRequest pageRequest)
        {
            Page<NetsuiteFormula> result = await netsuiteFormulaRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<NetsuiteFormula>, Page<NetsuiteFormulaDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormulaDto}}"/>.</returns>
        public async Task<Page<NetsuiteFormulaDto>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            FindRequestImpl<BaseSearchFilter> findRequest = filter.PerformMapping<FindRequestImpl<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            Page<NetsuiteFormula> result = await this.netsuiteFormulaRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<NetsuiteFormula>, Page<NetsuiteFormulaDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaDto"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<NetsuiteFormulaDto> SaveAsync(NetsuiteFormulaDto entity)
        {
            NetsuiteFormula formula = entity.PerformMapping<NetsuiteFormulaDto, NetsuiteFormula>();
            NetsuiteFormula result = await netsuiteFormulaRepository.SaveAsync(formula).ConfigureAwait(false);

            // handle steps
            result.Steps = await this.HandleSteps(entity, result).ConfigureAwait(false);

            return result.PerformMapping<NetsuiteFormula, NetsuiteFormulaDto>(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{NetsuiteFormulaDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<NetsuiteFormulaDto> entity)
        {
            return this.netsuiteFormulaRepository.SaveAsync(entity.PerformMapping<IList<NetsuiteFormulaDto>, IList<NetsuiteFormula>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaDto"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<NetsuiteFormulaDto> UpdateAsync(NetsuiteFormulaDto entity)
        {
            NetsuiteFormula storedFormula = await netsuiteFormulaRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            NetsuiteFormula formula = entity.PerformMapping(storedFormula);
            NetsuiteFormula result = await netsuiteFormulaRepository.UpdateAsync(formula).ConfigureAwait(false);

            // handle steps
            result.Steps = await this.HandleSteps(entity, result).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The HandleSteps.
        /// </summary>
        /// <param name="source">The source<see cref="NetsuiteFormulaDto"/>.</param>
        /// <param name="destination">The destination<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStep}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<IList<NetsuiteFormulaStep>> HandleSteps(NetsuiteFormulaDto source, NetsuiteFormula destination)
        {
            IList<NetsuiteFormulaStep> steps = source.Steps.PerformMapping<IList<NetsuiteFormulaStepDto>, IList<NetsuiteFormulaStep>>(); //Se pierden los lotes
            steps = steps ?? new List<NetsuiteFormulaStep>();
            IList<NetsuiteFormulaStep> actualSteps = destination.Steps ?? new List<NetsuiteFormulaStep>();
            // DeleteAsync no longer Steps
            IList<NetsuiteFormulaStep> removedSteps = actualSteps;
            if (removedSteps.Any())
            {
                removedSteps = removedSteps.Except(steps).ToList();
                foreach (NetsuiteFormulaStep step in removedSteps)
                {
                    NetsuiteFormulaStep toDelete = await netsuiteFormulaStepRepository.GetAsync(step.Id).ConfigureAwait(false);
                    if (toDelete != null)
                    {
                        await netsuiteFormulaStepRepository.DeleteAsync(toDelete).ConfigureAwait(false);
                    }
                }
            }

            // Edited steps are managed at the moment so it is not necessary take care about them here

            IList<NetsuiteFormulaStep> newSteps = steps.Where(x => x.Id == default(Guid)).ToList();

            // Add new Steps
            if (newSteps.Any())
            {
                foreach (NetsuiteFormulaStep step in newSteps)
                {
                    step.Formula = destination;
                    // Save on BBDD
                    NetsuiteFormulaStep stepSaved = await netsuiteFormulaStepRepository.SaveAsync(step).ConfigureAwait(false);
                }
            }

            return destination.Steps;
        }
    }
}
