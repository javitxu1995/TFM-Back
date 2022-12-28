namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.Formulas;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaRepository" />.
    /// </summary>
    internal class NetsuiteFormulaRepository : NHibernateRepository, INetsuiteFormulaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public NetsuiteFormulaRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{NetsuiteFormula}}"/>.</returns>
        public new Task<IList<NetsuiteFormula>> GetAllAsync()
        {
            return GetAllAsync<NetsuiteFormula>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public Task<NetsuiteFormula> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<NetsuiteFormula>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormula}}"/>.</returns>
        public Task<Page<NetsuiteFormula>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<NetsuiteFormula>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{NetsuiteFormula}}"/>.</returns>
        public new Task<Page<NetsuiteFormula>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<NetsuiteFormula, NetsuiteFormula> qo = CurrentSession.QueryOver<NetsuiteFormula>();

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

            return PaginatedAsync(qo, filter.PageRequest);
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
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{NetsuiteFormula}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<NetsuiteFormula> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="NetsuiteFormula"/>.</param>
        /// <returns>The <see cref="Task{NetsuiteFormula}"/>.</returns>
        public async Task<NetsuiteFormula> UpdateAsync(NetsuiteFormula entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
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
