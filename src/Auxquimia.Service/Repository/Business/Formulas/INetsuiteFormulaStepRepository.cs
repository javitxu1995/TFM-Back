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
    /// Defines the <see cref="INetsuiteFormulaStepRepository" />.
    /// </summary>
    public interface INetsuiteFormulaStepRepository : IRepositoryBase<NetsuiteFormulaStep>, ISupportsDelete<NetsuiteFormulaStep>, ISupportsSave<NetsuiteFormulaStep, Guid>, ISearcheable<NetsuiteFormulaStep, BaseSearchFilter> 
    {
        /// <summary>
        /// The UpdateStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        Task<NetsuiteFormulaStep> UpdateStepWithSession(ISession session, NetsuiteFormulaStep step);

        /// <summary>
        /// The SaveStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        Task<NetsuiteFormulaStep> SaveStepWithSession(ISession session, NetsuiteFormulaStep step);

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        Task<NetsuiteFormulaStep> GetAsyncWithSession(ISession session, Guid stepId);

        /// <summary>
        /// The GetByStepAndLotWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        Task<NetsuiteFormulaStep> GetByStepAndLotWithSession(ISession session, int step, string lot);

        /// <summary>
        /// The GetByStepAndLot.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        Task<NetsuiteFormulaStep> GetByStepAndLot(int step, string lot);

        /// <summary>
        /// The FindOtherLotsWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <param name="additionSequence">The additionSequence<see cref="int"/>.</param>
        /// <param name="lotToExclude">The lotToExclude<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStep}}"/>.</returns>
        Task<IList<NetsuiteFormulaStep>> FindOtherLotsWithSession(ISession session, Guid formulaId, int additionSequence, string lotToExclude);

    }
}
