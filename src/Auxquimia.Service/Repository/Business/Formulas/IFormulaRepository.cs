namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Izertis.Interfaces.Abstractions;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFormulaRepository" />.
    /// </summary>
    public interface IFormulaRepository : ISupportsSave<Formula, Guid>, IDao<Formula, Guid>, ISearchableDao<Formula, BaseSearchFilter>
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
        Task<Page<Formula>> GetForAssembly(FindRequestImpl<BaseSearchFilter> filter);

        /// <summary>
        /// The FindNotOnProduction.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Formula}}"/>.</returns>
        Task<Page<Formula>> FindNotOnProduction(FindRequestImpl<BaseSearchFilter> filter);
    }
}
