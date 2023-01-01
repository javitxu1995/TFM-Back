namespace Auxquimia.Service.Authentication
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Servs;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRoleService" />.
    /// </summary>
    public interface IRoleService : IService<RoleDto, Guid>, ISupportsSave<RoleDto, Guid>, ISearcheableRepository<RoleDto, RoleSearchFilter>
    {
        /// <summary>
        /// The getAdminRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        Task<RoleDto> getAdminRole();

        /// <summary>
        /// The getManagerRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        Task<RoleDto> getManagerRole();

        /// <summary>
        /// The getUserRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        Task<RoleDto> getUserRole();
    }
}
