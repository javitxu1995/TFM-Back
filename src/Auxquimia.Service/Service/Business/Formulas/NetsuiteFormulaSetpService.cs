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
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaSetpService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class NetsuiteFormulaSetpService : INetsuiteFormulaStepService
    {
        /// <summary>
        /// Defines the netsuiteFormulaStepRepository.
        /// </summary>
        private readonly INetsuiteFormulaStepRepository netsuiteFormulaStepRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaSetpService"/> class.
        /// </summary>
        /// <param name="netsuiteFormulaStepRepository">The netsuiteFormulaStepRepository<see cref="INetsuiteFormulaStepRepository"/>.</param>
        public NetsuiteFormulaSetpService(INetsuiteFormulaStepRepository netsuiteFormulaStepRepository)
        {
            this.netsuiteFormulaStepRepository = netsuiteFormulaStepRepository;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStepDto}}"/>.</returns>
        public async Task<IList<NetsuiteFormulaStepDto>> GetAllAsync()
        {
            var result = await netsuiteFormulaStepRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<NetsuiteFormulaStep>, IList<NetsuiteFormulaStepDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        public async Task<NetsuiteFormulaStepDto> GetAsync(Guid id)
        {
            var result = await netsuiteFormulaStepRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<NetsuiteFormulaStep, NetsuiteFormulaStepDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormulaStepDto}}"/>.</returns>
        public async Task<Page<NetsuiteFormulaStepDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await netsuiteFormulaStepRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<NetsuiteFormulaStep>, Page<NetsuiteFormulaStepDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormulaStepDto}}"/>.</returns>
        public async Task<Page<NetsuiteFormulaStepDto>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestImpl<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            var result = await netsuiteFormulaStepRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<NetsuiteFormulaStep>, Page<NetsuiteFormulaStepDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<NetsuiteFormulaStepDto> SaveAsync(NetsuiteFormulaStepDto entity)
        {
            NetsuiteFormulaStep step = entity.PerformMapping<NetsuiteFormulaStepDto, NetsuiteFormulaStep>();
            NetsuiteFormulaStep result = await netsuiteFormulaStepRepository.SaveAsync(step).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{NetsuiteFormulaStepDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<NetsuiteFormulaStepDto> entity)
        {
            return this.netsuiteFormulaStepRepository.SaveAsync(entity.PerformMapping<IList<NetsuiteFormulaStepDto>, IList<NetsuiteFormulaStep>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<NetsuiteFormulaStepDto> UpdateAsync(NetsuiteFormulaStepDto entity)
        {
            NetsuiteFormulaStep storedFormulaStep = await netsuiteFormulaStepRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            NetsuiteFormulaStep formulaStep = entity.PerformMapping(storedFormulaStep);
            NetsuiteFormulaStep result = await netsuiteFormulaStepRepository.UpdateAsync(formulaStep).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The GetNextUnwritedStep.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        public Task<NetsuiteFormulaStepDto> GetNextUnwritedStep(int step, Guid formulaId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The MarkStepAsWritted.
        /// </summary>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<NetsuiteFormulaStepDto> MarkStepAsWritted(Guid stepId, ISession session = null)
        {
            NetsuiteFormulaStep step = null;
            if (session != null)
            {
                step = await this.netsuiteFormulaStepRepository.GetAsyncWithSession(session, stepId);
            }
            else
            {
                step = await this.netsuiteFormulaStepRepository.GetAsync(stepId);
            }
           
            if (step != null)
            {
                step.Written = true;
                if (session != null)
                {
                    step = await this.netsuiteFormulaStepRepository.UpdateStepWithSession(session, step);
                }
                else
                {
                    step = await this.netsuiteFormulaStepRepository.UpdateAsync(step).ConfigureAwait(false);
                }
                return step.PerformMapping<NetsuiteFormulaStep, NetsuiteFormulaStepDto>();
            }
            return null;
        }
    }
}
