namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using Auxquimia.Utils.MVC.Tools.Servs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFormulaService" />.
    /// </summary>
    public interface IFormulaService : IService<FormulaDto, Guid>, ISupportsSave<FormulaDto, Guid>, ISearchableService<FormulaDto, BaseSearchFilter>
    {
        /// <summary>
        /// The GetForAssembly.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        Task<IList<FormulaDto>> GetForAssembly(FindRequestImpl<BaseSearchFilter> filter);

        /// <summary>
        /// The FindNotOnProduction.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaDto}}"/>.</returns>
        Task<IList<FormulaDto>> FindNotOnProduction(FindRequestImpl<BaseSearchFilter> filter);
    }
}
