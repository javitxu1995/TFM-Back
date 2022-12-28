namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Izertis.Interfaces.Abstractions;
    using NHibernate;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="INetsuiteFormulaStepService" />.
    /// </summary>
    public interface INetsuiteFormulaStepService : IService<NetsuiteFormulaStepDto, Guid>, ISupportsSave<NetsuiteFormulaStepDto, Guid>, ISearchableService<NetsuiteFormulaStepDto, BaseSearchFilter>
    {
        /// <summary>
        /// The MarkStepAsWritted.
        /// </summary>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        Task<NetsuiteFormulaStepDto> MarkStepAsWritted(Guid stepId, ISession session = null);

        /// <summary>
        /// The GetNextUnwritedStep.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStepDto}"/>.</returns>
        Task<NetsuiteFormulaStepDto> GetNextUnwritedStep(int step, Guid formulaId);
    }
}
