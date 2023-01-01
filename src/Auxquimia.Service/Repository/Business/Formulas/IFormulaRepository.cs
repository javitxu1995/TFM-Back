namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFormulaRepository" />.
    /// </summary>
    public interface IFormulaRepository : IRepositoryBase<Formula>, ISupportsSave<Formula, Guid>, ISearcheable<Formula, BaseSearchFilter>
    {
        /// <summary>
        /// The UpdateFormulaWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        Task<Formula> UpdateFormulaWithSession(ISession session, Formula entity);

        /// <summary>
        /// The GetForAssembly.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Formula}}"/>.</returns>
        Task<IList<Formula>> GetForAssembly(FindRequestImpl<BaseSearchFilter> filter);

        /// <summary>
        /// The FindNotOnProduction.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Formula}}"/>.</returns>
        Task<IList<Formula>> FindNotOnProduction(FindRequestImpl<BaseSearchFilter> filter);
    }
}
