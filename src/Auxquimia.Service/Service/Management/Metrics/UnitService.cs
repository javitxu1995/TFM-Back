namespace Auxquimia.Service.Management.Metrics
{
    using Auxquimia.Dto.Management.Metrics;
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Management.Metrics;
    using Auxquimia.Repository.Management.Metrics;
    using Auxquimia.Utils;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UnitService" />.
    /// </summary>
    internal class UnitService : IUnitService
    {
        /// <summary>
        /// Defines the unitRepository.
        /// </summary>
        private readonly IUnitRepository unitRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitService"/> class.
        /// </summary>
        /// <param name="unitRepository">The unitRepository<see cref="IUnitRepository"/>.</param>
        public UnitService(IUnitRepository unitRepository)
        {
            this.unitRepository = unitRepository;
        }

        /// <summary>
        /// The FindByCode.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        public async Task<UnitDto> FindByCode(string code)
        {
            if (!StringUtils.HasText(code))
            {
                return null;
            }
            Unit unit = await this.unitRepository.FindByCode(code);
            return unit.PerformMapping<Unit, UnitDto>();
        }

        /// <summary>
        /// The FindByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        public async Task<UnitDto> FindByName(string name)
        {
            if (!StringUtils.HasText(name))
            {
                return null;
            }
            Unit unit = await this.unitRepository.FindByName(name);
            return unit.PerformMapping<Unit, UnitDto>();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{UnitDto}}"/>.</returns>
        public async Task<IList<UnitDto>> GetAllAsync()
        {
            IList<Unit> result = await unitRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Unit>, IList<UnitDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        public async Task<UnitDto> GetAsync(Guid id)
        {
            Unit result = await unitRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Unit, UnitDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UnitDto}}"/>.</returns>
        public async Task<IList<UnitDto>> SearchByFilter(FindRequestImpl<BaseSearchFilter> filter)
        {
            FindRequestImpl<BaseSearchFilter> findRequest = filter.PerformMapping<FindRequestImpl<BaseSearchFilter>, FindRequestImpl<BaseSearchFilter>>();
            IList<Unit> result = await this.unitRepository.SearchByFilter(findRequest).ConfigureAwait(false);
            return result.PerformMapping<IList<Unit>, IList<UnitDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UnitDto"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        public async Task<UnitDto> SaveAsync(UnitDto entity)
        {
            Unit unit = entity.PerformMapping<UnitDto, Unit>();
            Unit result = await unitRepository.SaveAsync(unit).ConfigureAwait(false);


            return result.PerformMapping<Unit, UnitDto>(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{UnitDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<UnitDto> entity)
        {
            foreach(UnitDto unit in entity)
            {
                await SaveAsync(unit).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UnitDto"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        public async Task<UnitDto> UpdateAsync(UnitDto entity)
        {
            Unit storedUnit = await unitRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Unit formula = entity.PerformMapping(storedUnit);
            Unit result = await unitRepository.UpdateAsync(formula).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }
    }
}
