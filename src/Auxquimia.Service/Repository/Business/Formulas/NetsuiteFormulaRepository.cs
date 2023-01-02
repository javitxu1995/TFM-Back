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
    /// Defines the <see cref="NetsuiteFormulaRepository" />.
    /// </summary>
    internal class NetsuiteFormulaRepository : RepositoryBase<NetsuiteFormula>, INetsuiteFormulaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public NetsuiteFormulaRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{NetsuiteFormula}}"/>.</returns>
        public override Task<IList<NetsuiteFormula>> GetAllAsync()
        {
            return _session.QueryOver<NetsuiteFormula>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public override Task<NetsuiteFormula> GetAsync(Guid id)
        {
            return _session.QueryOver<NetsuiteFormula>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormula}}"/>.</returns>
        public new Task<IList<NetsuiteFormula>> SearchByFilter(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<NetsuiteFormula, NetsuiteFormula> qo = _session.QueryOver<NetsuiteFormula>();

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
        /// <param name="entity">The entity<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public async Task<NetsuiteFormula> SaveAsync(NetsuiteFormula entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public async override Task<NetsuiteFormula> UpdateAsync(NetsuiteFormula entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateFormulaWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public async Task<NetsuiteFormula> UpdateFormulaWithSession(ISession session, NetsuiteFormula entity)
        {
            return await session.MergeAsync(entity).ConfigureAwait(false);
        }

    }
}
