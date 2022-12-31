namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Model.Authentication;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UserRepository" />.
    /// </summary>
    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="sessionFactoryProvider">Session factory provider.</param>
        public UserRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<User> SaveAsync(User entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task SaveAsync(IList<User> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<User> UpdateAsync(User entity)
        {
            await base.UpdateAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>.</returns>
        public Task<User> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<User>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>.</returns>
        public Task<IList<User>> GetAllAsync()
        {
            return GetAllAsync<User>();
        }

        /// <summary>
        /// Paginateds the asynchronous.
        /// </summary>
        /// <param name="pageRequest">The page request.</param>
        /// <returns>.</returns>
        public Task<Page<User>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<User>(), pageRequest);
        }

        /// <summary>
        /// Paginateds the asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>.</returns>
        public Task<Page<User>> PaginatedAsync(FindRequestImpl<UserSearchFilter> filter)
        {
            IQueryOver<User, User> qo = CurrentSession.QueryOver<User>();

            if (filter.Filter != null)
            {
                UserSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Email))
                {
                    qo.And(Restrictions.On<User>(x => x.Email).IsInsensitiveLike(uFilter.Email, MatchMode.Anywhere));
                }

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<User>(x => x.Username).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

                if (uFilter.Enabled != null)
                {
                    qo.And(x => x.AccountNonLocked == uFilter.Enabled);
                }

                if (uFilter.FactoryId != null)
                {
                    qo.And(x => x.Factory != null).And(x => x.Factory.Id == uFilter.FactoryId);
                }
            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// Finds the by username and password asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>.</returns>
        public Task<User> FindByUsernameAndPasswordAsync(string username, string password)
        {
            return CurrentSession.QueryOver<User>().Where(x => x.Username == username).And(x => x.Password == password).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds the by username asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>.</returns>
        public Task<User> FindByUsernameAsync(string username)
        {
            return CurrentSession.QueryOver<User>().Where(x => x.Username == username).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Toggles the enabled user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns>.</returns>
        public async Task ToggleEnabledUserAsync(Guid userId, bool enabled)
        {
            User user = await GetAsync(userId);
            user.AccountNonLocked = enabled;
            await UpdateAsync(user);
        }

        /// <summary>
        /// The SearchHighUsers.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{User}}"/>.</returns>
        public Task<Page<User>> SearchHighUsers(FindRequestImpl<UserSearchFilter> filter)
        {
            User userAlias = null;
            UserRole userRoleAlias = null;
            Role roleAlias = null;

            IQueryOver<User, User> qo = CurrentSession.QueryOver(() => userAlias)
                .JoinEntityAlias(() => userRoleAlias, () => userRoleAlias.User.Id == userAlias.Id)
                .JoinAlias(() => userRoleAlias.Role, () => roleAlias);

            if (filter.Filter != null)
            {
                UserSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Email))
                {
                    qo.And(Restrictions.On<User>(x => x.Email).IsInsensitiveLike(uFilter.Email, MatchMode.Anywhere));
                }

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<User>(x => x.Username).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }

                if (uFilter.Enabled != null)
                {
                    qo.And(x => x.AccountNonLocked == uFilter.Enabled);
                }

                if (uFilter.FactoryId != null)
                {
                    qo.And(x => x.Factory != null).And(x => x.Factory.Id == uFilter.FactoryId);
                }
            }

            qo.And(Restrictions.Where(() => roleAlias.AbSelectable));


            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The FindbyCode.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        public Task<IList<User>> FindbyCode(int code, Guid factoryId)
        {
            return CurrentSession.QueryOver<User>().Where(x => x.Code == code && x.Factory.Id == factoryId).ListAsync();
        }

        /// <summary>
        /// The IsCodeAvailable.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<IList<User>> IsCodeAvailable(int code, Guid id = default(Guid))
        {

            IQueryOver<User, User> qo = CurrentSession.QueryOver<User>();
            if (id == default(Guid))
            {
                qo.Where(x => x.Code == code); //New Operator
            }
            else
            {
                qo.Where(x => x.Id != id && x.Code == code); //Updating
            }
            IList<User> users = await qo.ListAsync();
            return users;
        }

        /// <summary>
        /// The SearchForSelect.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{User}}"/>.</returns>
        public Task<Page<User>> SearchForSelect(FindRequestImpl<UserSearchFilter> filter)
        {
            IQueryOver<User, User> qo = CurrentSession.QueryOver<User>();

            if (filter.Filter != null)
            {
                UserSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<User>(x => x.Username).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere) ||
                        Restrictions.On<User>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere) ||
                        Restrictions.On<User>(x => x.Surname).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }
            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The FindByEmailAsync.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        public Task<User> FindByEmailAsync(string email)
        {
            return CurrentSession.QueryOver<User>().Where(x => x.Email == email && x.Enabled).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The FindByPasswordToken.
        /// </summary>
        /// <param name="token">The token<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        public Task<User> FindByPasswordToken(Guid token)
        {
            long today = DateHelper.GetTodayUnixTimeMilliseconds();
            return CurrentSession.QueryOver<User>().Where(x => x.PasswordToken == token && x.PasswordTokenExpiration >= today && x.Enabled).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The FindByCodeAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        public Task<User> FindByCodeAsyncWithSession(ISession session, int code, Guid factoryId)
        {
            return session.QueryOver<User>().Where(x => x.Code == code && x.Factory.Id == factoryId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The FindByUsernameAndFactoryAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        public Task<User> FindByUsernameAndFactoryAsyncWithSession(ISession session, string username, Guid factoryId)
        {
            return session.QueryOver<User>().Where(x => x.Username == username && x.Factory.Id == factoryId).SingleOrDefaultAsync();
        }
    }
}
