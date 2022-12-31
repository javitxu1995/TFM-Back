namespace Auxquimia.Repository.Management.Metrics
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Metrics;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UnitRepository" />.
    /// </summary>
    internal class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public UnitRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public override Task<Unit> GetAsync(Guid id)
        {
            return _session.QueryOver<Unit>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Unit}}"/>.</returns>
        public Task<IList<Unit>> SearchByFilter(BaseSearchFilter filter)
        {
            IQueryOver<Unit, Unit> qo = _session.QueryOver<Unit>();

            if (filter != null)
            {

                if (StringUtils.HasText(filter.Code))
                {
                    qo.And(Restrictions.On<Unit>(x => x.Code).IsInsensitiveLike(filter.Code, MatchMode.Anywhere));
                }
                if (StringUtils.HasText(filter.Name))
                {
                    qo.And(Restrictions.On<Unit>(x => x.Name).IsInsensitiveLike(filter.Name, MatchMode.Anywhere));
                }

            }

            return qo.ListAsync();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Unit"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public async Task<Unit> SaveAsync(Unit entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }


        public Task<Unit> FindByCode(string code)
        {
            return _session.QueryOver<Unit>().Where(x => x.Code == code).SingleOrDefaultAsync();
        }

        public Task<Unit> FindByName(string name)
        {
            return _session.QueryOver<Unit>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Unit"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        public override async Task<Unit> UpdateAsync(Unit entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }

        public override Task<IList<Unit>> GetAllAsync()
        {
            return GetAllAsync();
        }
    }
}
