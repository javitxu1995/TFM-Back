namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUserRoleRepository" />.
    /// </summary>
    interface IUserRoleRepository : IRepositoryBase<UserRole>, ISupportsDelete<UserRole>, ISupportsSave<UserRole, Guid>, ISearcheable<UserRole, RoleSearchFilter>
    //ISupportsSave<UserRole, Guid>, IDao<UserRole, Guid>, ISearchableDao<UserRole, RoleSearchFilter>, ISupportsDelete<UserRole, Guid>
    {
        /// <summary>
        /// The GetByUserIdAndRoleId.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="roleId">The roleId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserRole}"/>.</returns>
        Task<UserRole> GetByUserIdAndRoleId(Guid userId, Guid roleId);
    }
}
