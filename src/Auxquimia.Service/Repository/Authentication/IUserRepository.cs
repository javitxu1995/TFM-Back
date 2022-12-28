namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Model.Authentication;
    using Auxquimia.Service.Filters.Authentication;
    using Izertis.Interfaces.Abstractions;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUserRepository" />.
    /// </summary>
    public interface IUserRepository : ISupportsSave<User, Guid>, IDao<User, Guid>, ISearchableDao<User, UserSearchFilter>
    {
        /// <summary>
        /// The FindByUsernameAndPasswordAsync.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="password">The password<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByUsernameAndPasswordAsync(string username, string password);

        /// <summary>
        /// The FindByUsernameAsync.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByUsernameAsync(string username);

        /// <summary>
        /// The FindByEmailAsync.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByEmailAsync(string email);

        /// <summary>
        /// The ToggleEnabledUserAsync.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="enabled">The enabled<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ToggleEnabledUserAsync(Guid userId, bool enabled);

        /// <summary>
        /// The SearchHighUsers.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{User}}"/>.</returns>
        Task<Page<User>> SearchHighUsers(FindRequestImpl<UserSearchFilter> filter);

        /// <summary>
        /// The SearchForSelect.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{User}}"/>.</returns>
        Task<Page<User>> SearchForSelect(FindRequestImpl<UserSearchFilter> filter);

        /// <summary>
        /// The FindbyCode.
        /// </summary>
        /// <param name="Code">The Code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<IList<User>> FindbyCode(int Code, Guid factoryId);

        /// <summary>
        /// The IsCodeAvailable.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{User}}"/>.</returns>
        Task<IList<User>> IsCodeAvailable(int code, Guid id = default(Guid));

        /// <summary>
        /// The FindByPasswordToken.
        /// </summary>
        /// <param name="token">The token<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByPasswordToken(Guid token);

        /// <summary>
        /// The FindByCodeAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByCodeAsyncWithSession(ISession session, int code, Guid factoryId);

        /// <summary>
        /// The FindByUsernameAndFactoryAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> FindByUsernameAndFactoryAsyncWithSession(ISession session, string username, Guid factoryId);
    }
}
