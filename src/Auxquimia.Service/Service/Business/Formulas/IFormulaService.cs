namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Izertis.Interfaces.Abstractions;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFormulaService" />.
    /// </summary>
    public interface IFormulaService : IService<FormulaDto, Guid>, ISupportsSave<FormulaDto, Guid>, ISearchableService<FormulaDto, BaseSearchFilter>
    {
        /// <summary>
        /// The GetForAssembly.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        Task<Page<FormulaDto>> GetForAssembly(FindRequestDto<BaseSearchFilter> filter);

        /// <summary>
        /// The FindNotOnProduction.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        Task<Page<FormulaDto>> FindNotOnProduction(FindRequestDto<BaseSearchFilter> filter);
    }
}
