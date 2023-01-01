namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStepRepository" />.
    /// </summary>
    internal class NetsuiteFormulaStepRepository : RepositoryBase<NetsuiteFormulaStep>, INetsuiteFormulaStepRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaStepRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public NetsuiteFormulaStepRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<NetsuiteFormulaStep> DeleteAsync(NetsuiteFormulaStep entity)
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
            IQuery query = _session.CreateQuery("delete NETSUITE_FORMULA_STEP where Id = :id");
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
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStep}}"/>.</returns>
        public Task<IList<NetsuiteFormulaStep>> FindOtherLotsWithSession(ISession session, Guid formulaId, int additionSequence, string lotToExclude)
        {
            return session.QueryOver<NetsuiteFormulaStep>().Where(x => x.Formula.Id == formulaId && x.AdditionSequence == additionSequence && x.InventoryLot != lotToExclude && x.Written).ListAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{NetsuiteFormulaStep}}"/>.</returns>
        public override Task<IList<NetsuiteFormulaStep>> GetAllAsync()
        {
            return _session.QueryOver<NetsuiteFormulaStep>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public override Task<NetsuiteFormulaStep> GetAsync(Guid id)
        {
            return _session.QueryOver<NetsuiteFormulaStep>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="stepId">The stepId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public Task<NetsuiteFormulaStep> GetAsyncWithSession(ISession session, Guid stepId)
        {
            return session.QueryOver<NetsuiteFormulaStep>().Where(x => x.Id == stepId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByStepAndLot.
        /// </summary>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public Task<NetsuiteFormulaStep> GetByStepAndLot(int step, string lot)
        {
            return _session.QueryOver<NetsuiteFormulaStep>().Where(x => x.AdditionSequence == step && x.InventoryLot == lot).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByStepAndLotWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="int"/>.</param>
        /// <param name="lot">The lot<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public Task<NetsuiteFormulaStep> GetByStepAndLotWithSession(ISession session, int step, string lot)
        {
            return session.QueryOver<NetsuiteFormulaStep>().Where(x => x.AdditionSequence == step && x.InventoryLot == lot).SingleOrDefaultAsync();
        }


        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormulaStep}}"/>.</returns>
        public Task<IList<NetsuiteFormulaStep>> SearchByFilter(FindRequestDto<BaseSearchFilter> filter)
        {
            IQueryOver<NetsuiteFormulaStep, NetsuiteFormulaStep> qo = _session.QueryOver<NetsuiteFormulaStep>();

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
        /// <param name="entity">The entity<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public async Task<NetsuiteFormulaStep> SaveAsync(NetsuiteFormulaStep entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public async Task<NetsuiteFormulaStep> SaveStepWithSession(ISession session, NetsuiteFormulaStep step)
        {
            await session.SaveAsync(step).ConfigureAwait(false);
            return step;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public async override Task<NetsuiteFormulaStep> UpdateAsync(NetsuiteFormulaStep entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateStepWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="step">The step<see cref="NetsuiteFormulaStep"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormulaStep}"/>.</returns>
        public async Task<NetsuiteFormulaStep> UpdateStepWithSession(ISession session, NetsuiteFormulaStep step)
        {
            return await session.MergeAsync(step).ConfigureAwait(false);
        }
    }
}
