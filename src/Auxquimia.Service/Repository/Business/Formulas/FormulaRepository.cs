namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Enums;
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FormulaRepository" />.
    /// </summary>
    internal class FormulaRepository : RepositoryBase<Formula>, IFormulaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FormulaRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        public Task<IList<Formula>> FindNotOnProduction(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = _session.QueryOver<Formula>();
            AssemblyBuild assemblyAlias = null;

            qo.Left.JoinAlias(x => x.AssemblyBuild, () => assemblyAlias);
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
            //qo.Left.JoinQueryOver(x => x.AssemblyBuild).Where(a => a.Status == ABStatus.PENDING);
  
            qo.And(x => x.AssemblyBuild == null || assemblyAlias.Status == ABStatus.PENDING);
            //qo.JoinQueryOver(x => x.AssemblyBuild).Where(a => a.Status == ABStatus.PENDING);
            //qo.Join(x => x.AssemblyBuild == null);
            return qo.ListAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Formula}}"/>.</returns>
        public override Task<IList<Formula>> GetAllAsync()
        {
            return _session.QueryOver<Formula>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public override Task<Formula> GetAsync(Guid id)
        {
            return _session.QueryOver<Formula>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public Task<IList<Formula>> GetForAssembly(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = _session.QueryOver<Formula>();

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
            qo.And(x => x.AssemblyBuild == null);

            return qo.ListAsync();
        }

        
        public Task<IList<Formula>> SearchByFilter(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = _session.QueryOver<Formula>();

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
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public async Task<Formula> SaveAsync(Formula entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public async override Task<Formula> UpdateAsync(Formula entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        public async Task<Formula> UpdateFormulaWithSession(ISession session, Formula entity)
        {
            return await session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
