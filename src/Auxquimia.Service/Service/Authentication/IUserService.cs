namespace Auxquimia.Service.Authentication
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using Auxquimia.Utils.MVC.Tools.Servs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Service to handle Operator entity related operations.
    /// </summary>
    public interface IUserService : IService<UserDto, Guid>, ISupportsSave<UserDto, Guid>, ISearcheableService<UserDto, UserSearchFilter>
    {
        /// <summary>
        /// Finds the by username and password asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>.</returns>
        Task<UserDto> FindByUsernameAndPasswordAsync(string username, string password);

        /// <summary>
        /// Finds the by username asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>.</returns>
        Task<UserDto> FindByUsernameAsync(string username);

        /// <summary>
        /// The FindByEmailAsync.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        Task<UserDto> FindByEmailAsync(string email);

        /// <summary>
        /// Toggles the enabled user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns>.</returns>
        Task ToggleEnabledUserAsync(Guid userId, bool enabled);

        /// <summary>
        /// The SearchHighUsers.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        Task<IList<UserDto>> SearchHighUsers(FindRequestDto<UserSearchFilter> filter);

        /// <summary>
        /// The SearchForSelect.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        Task<IList<UserDto>> SearchForSelect(FindRequestDto<UserSearchFilter> filter);

        /// <summary>
        /// The FindByCode.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        Task<UserDto> FindByCode(int code, string factoryId);

        /// <summary>
        /// The IsCodeAvailable.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> IsCodeAvailable(int code, Guid factoryId);

        /// <summary>
        /// The ResetPasswordForUser.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> ResetPasswordForUser(Guid userId);

        /// <summary>
        /// The UpdatePasswordForUser.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="password">The password<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> UpdatePasswordForUser(Guid userId, string password);

        /// <summary>
        /// The FindByPasswordToken.
        /// </summary>
        /// <param name="token">The token<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        Task<UserDto> FindByPasswordToken(Guid token);
    }
}
