namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UserRoleRepository" />.
    /// </summary>
    internal class UserRoleRepository : NHibernateRepository, IUserRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public UserRoleRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserRole"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DeleteAsync(UserRole entity)
        {
            return CurrentSession.DeleteAsync(entity);
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public Task<int> DeleteAsync(Guid id)
        {
            IQuery query = CurrentSession.CreateQuery("delete M_USER_ROLE where Id = :id");
            query.SetGuid("id", id);

            return query.ExecuteUpdateAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{UserRole}}"/>.</returns>
        public Task<IList<UserRole>> GetAllAsync()
        {
            return GetAllAsync<UserRole>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public Task<UserRole> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<UserRole>().Where(x => x.Id == id).SingleOrDefaultAsync();
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

            IQueryOver<UserRole, UserRole> qo = CurrentSession.QueryOver(() => userRoleAlias)
                .JoinAlias(() => userRoleAlias.User, () => userAlias)
                .JoinAlias(() => userRoleAlias.Role, () => roleAlias)
                .Where(() => userAlias.Id == userId && roleAlias.Id == roleId);
            qo.Select(p => p.AsEntity());
            return qo.SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{UserRole}}"/>.</returns>
        public Task<Page<UserRole>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<UserRole>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{RoleSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserRole}}"/>.</returns>
        public Task<Page<UserRole>> PaginatedAsync(FindRequestImpl<RoleSearchFilter> filter)
        {
            IQueryOver<UserRole, UserRole> qo = CurrentSession.QueryOver<UserRole>();

            return PaginatedAsync(qo, filter.PageRequest);
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

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{UserRole}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<UserRole> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserRole"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        public async Task<UserRole> UpdateAsync(UserRole entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
