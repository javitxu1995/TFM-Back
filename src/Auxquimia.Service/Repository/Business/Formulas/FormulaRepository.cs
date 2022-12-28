namespace Auxquimia.Repository.Business.Formulas
{
    using Auxquimia.Enums;
    using Auxquimia.Filters;
    using Auxquimia.Model.Business.AssemblyBuilds;
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
    /// Defines the <see cref="FormulaRepository" />.
    /// </summary>
    internal class FormulaRepository : NHibernateRepository, IFormulaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FormulaRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        public Task<Page<Formula>> FindNotOnProduction(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = CurrentSession.QueryOver<Formula>();
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
                //if (uFilter.F)
                //{
                //    qo.And(Restrictions.On<Formula>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                //}

            }
            //qo.Left.JoinQueryOver(x => x.AssemblyBuild).Where(a => a.Status == ABStatus.PENDING);
  
            qo.And(x => x.AssemblyBuild == null || assemblyAlias.Status == ABStatus.PENDING);
            //qo.JoinQueryOver(x => x.AssemblyBuild).Where(a => a.Status == ABStatus.PENDING);
            //qo.Join(x => x.AssemblyBuild == null);
            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Formula}}"/>.</returns>
        public Task<IList<Formula>> GetAllAsync()
        {
            return GetAllAsync<Formula>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public Task<Formula> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<Formula>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public Task<Page<Formula>> GetForAssembly(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = CurrentSession.QueryOver<Formula>();

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

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{Formula}}"/>.</returns>
        public Task<Page<Formula>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Formula>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Formula}}"/>.</returns>
        public Task<Page<Formula>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<Formula, Formula> qo = CurrentSession.QueryOver<Formula>();

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
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public async Task<Formula> SaveAsync(Formula entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{Formula}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<Formula> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Formula"/>.</param>
        /// <returns>The <see cref="Task{Formula}"/>.</returns>
        public async Task<Formula> UpdateAsync(Formula entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }

        public async Task<Formula> UpdateFormulaWithSession(ISession session, Formula entity)
        {
            return await session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
