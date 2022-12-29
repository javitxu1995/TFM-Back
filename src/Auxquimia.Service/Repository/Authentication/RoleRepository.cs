namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RoleRepository" />.
    /// </summary>
    internal class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public RoleRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        public override Task Delete(Role entity)
        {
            return _session.DeleteAsync(entity);
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Role}}"/>.</returns>
        public override Task<IList<Role>> GetAllAsync()
        {
            return _session.QueryOver<Role>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public override Task<Role> GetAsync(Guid id)
        {
            return _session.QueryOver<Role>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The getByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public Task<Role> getByName(string name)
        {
            return _session.QueryOver<Role>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        ///// <summary>
        ///// The PaginatedAsync.
        ///// </summary>
        ///// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        ///// <returns>The <see cref="Task{Page{Role}}"/>.</returns>
        //public Task<Page<Role>> PaginatedAsync(PageRequest pageRequest)
        //{
        //    return PaginatedAsync(CurrentSession.QueryOver<Role>(), pageRequest);
        //}

        ///// <summary>
        ///// The PaginatedAsync.
        ///// </summary>
        ///// <param name="filter">The filter<see cref="FindRequestImpl{RoleSearchFilter}"/>.</param>
        ///// <returns>The <see cref="Task{Page{Role}}"/>.</returns>
        //public Task<Page<Role>> PaginatedAsync(FindRequestImpl<RoleSearchFilter> filter)
        //{
        //    IQueryOver<Role, Role> qo = CurrentSession.QueryOver<Role>();

        //    if (filter.Filter != null)
        //    {
        //        RoleSearchFilter uFilter = filter.Filter;

        //        if (StringUtils.HasText(uFilter.Name))
        //        {
        //            qo.And(Restrictions.On<Role>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
        //        }
        //    }

        //    return PaginatedAsync(qo, filter.PageRequest);
        //}


        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Role"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public async Task<Role> SaveAsync(Role entity)
        {
            await base.Save(entity).ConfigureAwait(false);
            return entity;
        }
        /// <summary>
        /// Update Async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task<Role> Update(Role entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
