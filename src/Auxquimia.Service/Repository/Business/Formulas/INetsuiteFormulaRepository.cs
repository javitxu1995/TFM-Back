namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Izertis.Interfaces.Abstractions;
    using NHibernate;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="INetsuiteFormulaRepository" />.
    /// </summary>
    public interface INetsuiteFormulaRepository : ISupportsSave<NetsuiteFormula, Guid>, IDao<NetsuiteFormula, Guid>, ISearchableDao<NetsuiteFormula, BaseSearchFilter>
    {
        /// <summary>
        /// The UpdateFormulaWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        Task<NetsuiteFormula> UpdateFormulaWithSession(ISession session, NetsuiteFormula entity);
    }
}
