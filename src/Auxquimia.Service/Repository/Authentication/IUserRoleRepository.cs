namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUserRoleRepository" />.
    /// </summary>
    interface IUserRoleRepository : ISupportsSave<UserRole, Guid>, IDao<UserRole, Guid>, ISearchableDao<UserRole, RoleSearchFilter>, ISupportsDelete<UserRole, Guid>
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
