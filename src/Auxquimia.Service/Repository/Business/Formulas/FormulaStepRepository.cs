namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FormulaStepRepository" />.
    /// </summary>
    internal class FormulaStepRepository : RepositoryBase<FormulaStep>, IFormulaStepRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaStepRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FormulaStepRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<FormulaStep> DeleteAsync(FormulaStep entity)
        {
            await _session.DeleteAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public Task<int> DeleteAsync(Guid id)
        {
            IQuery query = _session.CreateQuery("delete FORMULA_STEP where Id = :id");
            query.SetGuid("id", id);

            return query.ExecuteUpdateAsync();
        }

        /// <summary>
        /// The FindOtherLotsWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <param name="additionSequence">The additionSequence<see cref="int"/>.</param>
        /// <param name="lotToExclude">The lotToExclude<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        public Task<IList<FormulaStep>> FindOtherLotsWithSession(ISession session, Guid formulaId, int additionSequence, string lotToExclude)
        {
            return session.QueryOver<FormulaStep>().Where(x => x.Formula.Id == formulaId && x.Step == additionSequence && x.InventoryLot != lotToExclude && x.Written).ListAsync();
        }

        /// <summary>
        /// The FindStepsFromFormula.
        /// </summary>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        public Task<IList<FormulaStep>> FindStepsFromFormula(Guid formulaId)
        {
            return _session.QueryOver<FormulaStep>().Where(x => x.Formula.Id == formulaId).OrderBy(p => p.Step).Asc.ListAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FormulaStep}}"/>.</returns>
        public override Task<IList<FormulaStep>> GetAllAsync()
        {
            return _session.QueryOver<FormulaStep>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public override Task<FormulaStep> GetAsync(Guid id)
        {
            return _session.QueryOver<FormulaStep>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public Task<FormulaStep> GetAsyncWithSession(ISession session, Guid stepId)
        {
            return session.QueryOver<FormulaStep>().Where(x => x.Id == stepId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByStepAndLot.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public Task<FormulaStep> GetByStepAndLot(int step, string lot)
        {
            return _session.QueryOver<FormulaStep>().Where(x => x.Step == step && x.InventoryLot == lot).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByStepAndLotWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public Task<FormulaStep> GetByStepAndLotWithSession(ISession session, int step, string lot)
        {
            return session.QueryOver<FormulaStep>().Where(x => x.Step == step && x.InventoryLot == lot).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FormulaStep}}"/>.</returns>
        public Task<IList<FormulaStep>> SearchByFilter(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<FormulaStep, FormulaStep> qo = _session.QueryOver<FormulaStep>();

            if (filter.Filter != null)
            {
                BaseSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Code))
                {
                    qo.And(Restrictions.On<Formula>(x => x.Code).IsInsensitiveLike(uFilter.Code, MatchMode.Anywhere));
                }
                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Formula>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

            }

            return qo.ListAsync();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public async Task<FormulaStep> SaveAsync(FormulaStep entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public async Task<FormulaStep> SaveStepWithSession(ISession session, FormulaStep step)
        {
            await session.SaveAsync(step).ConfigureAwait(false);
            return step;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public async override Task<FormulaStep> UpdateAsync(FormulaStep entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="FormulaStep"/>.</param>
        /// <returns>The <see cref="Task{FormulaStep}"/>.</returns>
        public async Task<FormulaStep> UpdateStepWithSession(ISession session, FormulaStep step)
        {
            return await session.MergeAsync(step).ConfigureAwait(false);
        }
    }
}
