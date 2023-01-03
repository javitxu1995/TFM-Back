namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFormulaStepRepository" />.
    /// </summary>
    public interface IFormulaStepRepository : IRepositoryBase<FormulaStep>, ISupportsDelete<FormulaStep>, ISupportsSave<FormulaStep, Guid>, ISearchableRepository<FormulaStep, BaseSearchFilter>
    {
        /// <summary>
        /// The UpdateStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        Task<FormulaStep> UpdateStepWithSession(ISession session, FormulaStep step);

        /// <summary>
        /// The SaveStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        Task<FormulaStep> SaveStepWithSession(ISession session, FormulaStep step);

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        Task<FormulaStep> GetAsyncWithSession(ISession session, Guid stepId);

        /// <summary>
        /// The FindStepsFromFormula.
        /// </summary>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        Task<IList<FormulaStep>> FindStepsFromFormula(Guid formulaId);

        /// <summary>
        /// The GetByStepAndLotWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        Task<FormulaStep> GetByStepAndLotWithSession(ISession session, int step, string lot);

        /// <summary>
        /// The GetByStepAndLot.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        Task<FormulaStep> GetByStepAndLot(int step, string lot);

        /// <summary>
        /// The FindOtherLotsWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <param name="additionSequence">The additionSequence<see cref="int"/>.</param>
        /// <param name="lotToExclude">The lotToExclude<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        Task<IList<FormulaStep>> FindOtherLotsWithSession(ISession session, Guid formulaId, int additionSequence, string lotToExclude);

    }
}
