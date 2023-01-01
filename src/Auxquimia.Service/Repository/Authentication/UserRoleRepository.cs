namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UserRoleRepository" />.
    /// </summary>
    internal class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public UserRoleRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserRole"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<UserRole> DeleteAsync(UserRole entity)
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
            IQuery query = _session.CreateQuery("delete M_USER_ROLE where Id = :id");
            query.SetGuid("id", id);

            return query.ExecuteUpdateAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{UserRole}}"/>.</returns>
        public override Task<IList<UserRole>> GetAllAsync()
        {
            return _session.QueryOver<UserRole>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public override Task<UserRole> GetAsync(Guid id)
        {
            return _session.QueryOver<UserRole>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByUserIdAndRoleId.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="roleId">The roleId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public Task<UserRole> GetByUserIdAndRoleId(Guid userId, Guid roleId)
        {
            UserRole userRoleAlias = null;
            User userAlias = null;
            Role roleAlias = null;

            IQueryOver<UserRole, UserRole> qo = _session.QueryOver(() => userRoleAlias)
                .JoinAlias(() => userRoleAlias.User, () => userAlias)
                .JoinAlias(() => userRoleAlias.Role, () => roleAlias)
                .Where(() => userAlias.Id == userId && roleAlias.Id == roleId);
            qo.Select(p => p.AsEntity());
            return qo.SingleOrDefaultAsync();
        }



        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserRole"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public async Task<UserRole> SaveAsync(UserRole entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public Task<IList<UserRole>> SearchByFilter(FindRequestImpl<RoleSearchFilter> filter)
        {
            IQueryOver<UserRole, UserRole> qo = _session.QueryOver<UserRole>();

            return qo.ListAsync();
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserRole"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public async override Task<UserRole> UpdateAsync(UserRole entity)
        {
            await _session.MergeAsync(entity).ConfigureAwait(false);
            return entity;
        }
    }
}
