namespace Auxquimia.Service.Authentication
{
    using Auxquimia.Config;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Exceptions;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UserService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class UserService : IUserService
    {
        /// <summary>
        /// Gets or sets the userRepository................
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Defines the userRoleRepository.
        /// </summary>
        private readonly IUserRoleRepository userRoleRepository;

        /// <summary>
        /// Defines the contextConfigProvider.
        /// </summary>
        private readonly IContextConfigProvider contextConfigProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="contextConfigProvider">The contextConfigProvider<see cref="IContextConfigProvider"/>.</param>
        /// <param name="userRepository">The userRepository<see cref="IUserRepository"/>.</param>
        /// <param name="userRoleRepository">The userRoleRepository<see cref="IUserRoleRepository"/>.</param>
        public UserService(IContextConfigProvider contextConfigProvider, IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.contextConfigProvider = contextConfigProvider;
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        public async Task<Page<UserDto>> PaginatedAsync(FindRequestDto<UserSearchFilter> filter)
        {
            FindRequestImpl<UserSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<UserSearchFilter>, FindRequestImpl<UserSearchFilter>>();
            Page<User> result = await userRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<User>, Page<UserDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> GetAsync(Guid id)
        {
            var result = await userRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<User, UserDto>();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{UserDto}}"/>.</returns>
        public async Task<IList<UserDto>> GetAllAsync()
        {
            var result = await userRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<User>, IList<UserDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        public async Task<Page<UserDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await userRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<User>, Page<UserDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserDto"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<UserDto> SaveAsync(UserDto entity)
        {
            User result = new User();
            int code = entity.Code != null ? (int)entity.Code : default(int);
            if (await this.SaveUserSafe(code, default(Guid)))
            {
                User user = entity.PerformMapping<UserDto, User>();
                result = await userRepository.SaveAsync(user).ConfigureAwait(false);

                // handle roles
                result.Roles = await this.HandleRoles(entity, result).ConfigureAwait(false);
            }

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{UserDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<UserDto> entity)
        {
            return userRepository.SaveAsync(entity.PerformMapping<IList<UserDto>, IList<User>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="UserDto"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<UserDto> UpdateAsync(UserDto entity)
        {
            User result = new User();
            int code = entity.Code != null ? (int)entity.Code : default(int);
            if (await this.SaveUserSafe(code, entity.Id.PerformMapping<string, Guid>()))
            {
                User storedUser = await userRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
                User user = entity.PerformMapping(storedUser);
                result = await userRepository.UpdateAsync(user).ConfigureAwait(false);


                // handle roles
                result.Roles = await this.HandleRoles(entity, result).ConfigureAwait(false);

            }

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The FindByUsernameAndPasswordAsync.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="password">The password<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> FindByUsernameAndPasswordAsync(string username, string password)
        {
            var result = await userRepository.FindByUsernameAsync(username).ConfigureAwait(false);
            if (result != null && !CryptographyUtil.ValidateHashData(password, result.Password))
            {
                result = null;
            }
            return result.PerformMapping<User, UserDto>();
        }

        /// <summary>
        /// The FindByUsernameAsync.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> FindByUsernameAsync(string username)
        {
            var result = await userRepository.FindByUsernameAsync(username).ConfigureAwait(false);
            return result.PerformMapping<User, UserDto>();
        }

        /// <summary>
        /// The ToggleEnabledUserAsync.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="enabled">The enabled<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task ToggleEnabledUserAsync(Guid userId, bool enabled)
        {
            await userRepository.ToggleEnabledUserAsync(userId, enabled).ConfigureAwait(false);
        }

        /// <summary>
        /// The HandleReactors.
        /// </summary>
        /// <param name="source">The source<see cref="UserDto"/>.</param>
        /// <param name="destination">The destination<see cref="User"/>.</param>
        /// <returns>The <see cref="Task{IList{UserRole}}"/>.</returns>
        private async Task<IList<UserRole>> HandleRoles(UserDto source, User destination)
        {
            IList<Role> roles = source.Roles.PerformMapping<IList<RoleDto>, IList<Role>>();
            IList<Role> actualRoles = new List<Role>();
            foreach (UserRole userRole in destination.Roles)
            {
                actualRoles.Add(userRole.Role);
            }

            // DeleteAsync no longer Reactors
            IList<Role> removedRoles = actualRoles;
            if (removedRoles.Any())
            {
                removedRoles = removedRoles.Except(roles).ToList();
                foreach (Role role in removedRoles)
                {
                    UserRole userRole = new UserRole
                    {
                        Role = role,
                        User = destination
                    };



                    // DeleteAsync on BBDD
                    UserRole deleteFactoryReactor = await userRoleRepository.GetByUserIdAndRoleId(destination.Id, role.Id).ConfigureAwait(false);
                    await userRoleRepository.DeleteAsync(deleteFactoryReactor).ConfigureAwait(false);

                }
            }

            // Add new Reactors
            if (roles.Any())
            {
                IList<Role> newRoles = roles.Except(actualRoles).ToList();

                foreach (Role role in newRoles)
                {
                    UserRole userRole = new UserRole
                    {
                        Role = role
                    };
                    // Add on returned list
                    destination.AddRole(userRole);
                    // Save on BBDD
                    await userRoleRepository.SaveAsync(userRole).ConfigureAwait(false);

                }
            }


            return destination.Roles;
        }

        /// <summary>
        /// The SearchHighUsers.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        public async Task<Page<UserDto>> SearchHighUsers(FindRequestDto<UserSearchFilter> filter)
        {
            FindRequestImpl<UserSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<UserSearchFilter>, FindRequestImpl<UserSearchFilter>>();
            Page<User> result = await userRepository.SearchHighUsers(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<User>, Page<UserDto>>();
        }

        /// <summary>
        /// The SearchForSelect.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{UserSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{UserDto}}"/>.</returns>
        public async Task<Page<UserDto>> SearchForSelect(FindRequestDto<UserSearchFilter> filter)
        {
            FindRequestImpl<UserSearchFilter> findRequest = filter.PerformMapping<FindRequestDto<UserSearchFilter>, FindRequestImpl<UserSearchFilter>>();
            Page<User> result = await userRepository.SearchForSelect(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<User>, Page<UserDto>>();
        }

        /// <summary>
        /// The FindByCode.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> FindByCode(int code, string factoryId)
        {
            if (factoryId == null || factoryId == default(string))
            {
                throw new CustomException(Constants.Exceptions.ERROR_USER_NO_FACTORY);
            }
            Guid nFactoryId = factoryId.PerformMapping<string, Guid>();
            var result = await userRepository.FindbyCode(code, nFactoryId).ConfigureAwait(false);
            if (result.Count > 0)
            {
                User user = result.ElementAt(0);
                return user.PerformMapping<User, UserDto>();
            }
            return null;
        }

        /// <summary>
        /// The IsCodeAvailable.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> IsCodeAvailable(int code, Guid factoryId)
        {
            IList<User> userSearch = await this.userRepository.FindbyCode(code, factoryId);
            return userSearch == null || userSearch.Count > 0;
        }

        /// <summary>
        /// The SaveUserSafe.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<bool> SaveUserSafe(int code, Guid id = default(Guid))
        {

            if (id == default(Guid))
            {
                IList<User> users = await this.userRepository.IsCodeAvailable(code);
                bool isNotAvailable = users != null && users.Count > 0;

                //Creando usuario
                if (code != default(int) && isNotAvailable)
                {
                    throw new CustomException(Constants.Exceptions.ERROR_CODE_NOT_AVAILABLE);
                }

            }
            else
            {
                //Actualziando

                IList<User> users = await this.userRepository.IsCodeAvailable(code, id);
                bool isNotAvailable = users != null && users.Count > 0;
                if (code != default(int) && isNotAvailable)
                {

                    throw new CustomException(Constants.Exceptions.ERROR_CODE_NOT_AVAILABLE);
                }

            }
            return true;
        }

        /// <summary>
        /// The FindByEmailAsync.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> FindByEmailAsync(string email)
        {
            User user = await this.userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return user.PerformMapping<User, UserDto>();
        }

        /// <summary>
        /// The ResetPasswordForUser.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<bool> ResetPasswordForUser(Guid userId)
        {
            User user = await this.userRepository.GetAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.PasswordToken = Guid.NewGuid();
            long expirationTime = DateHelper.AddDaysToUnixTimeMilliseconds(DateHelper.GetTodayUnixTimeMilliseconds(), 1);
            user.PasswordTokenExpiration = expirationTime;
            User updated = await this.userRepository.UpdateAsync(user).ConfigureAwait(false);

            if (updated == null)
            {
                return false;
            }
            //Send email
            IList<string> destinationEmails = new List<string>();
            destinationEmails.Add(updated.Email);
            string subject = "New password generated.";
            bool inEnglish = updated.Language != null && !updated.Language.Equals("ESP");
            string emailBody = GenerateSecurePasswordEmailBody(inEnglish, updated.Username, updated.PasswordToken, contextConfigProvider.EmailResetURL);
            EmailUtils.SendEmail(contextConfigProvider, contextConfigProvider.EmailAddress, destinationEmails, subject, emailBody);
            return true;
        }

        /// <summary>
        /// The UpdatePasswordForUser.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <param name="password">The password<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<bool> UpdatePasswordForUser(Guid userId, string password)
        {
            User user = await this.userRepository.FindByPasswordToken(userId).ConfigureAwait(false);
            if (user == null || string.IsNullOrEmpty(password))
            {
                return false;
            }
            
            user.PasswordTokenExpiration = 0;
            user.Password = HelperMethods.GetHash(password);
            User userUpdated = await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
            if (userUpdated == null)
            {
                return false;
            }
            return true;
        }


        private string GenerateSecurePasswordEmailBody(bool inEnglish, string username, Guid token, string url)
        {
            string emailBody;
            string redirectURL = url + "/" + token;
            if (inEnglish)
            {
                emailBody = @"<html><body>
		                        <div class='container'>" +
                                  "<div>" +
                                      "<p> Dear user, "+ username +": </p>" +
                                      "<p> A password reset request has been received.</p>" +
                                      "<p> You must access the following link to change your password. <a href='" + redirectURL + "'>"+ redirectURL + "</a> " +
                                  "</div>" +
                              "</div>" +
                          "</body></html>";
            }
            else
            {
                emailBody = @"<html><body>
		                        <div class='container'>" +
                                  "<div>" +
                                      "<p> Estimado usuario, " + username + ": </p>" +
                                      "<p> Se ha recibido una solicitud de cambio de contraseña.</p>" +
                                      "<p> Debe acceder al siguiente enlace para modificar su contraseña. <a href='" + redirectURL + "'>" + redirectURL + "</a> " +
                                  "</div>" +
                              "</div>" +
                          "</body></html>";
            }

            return emailBody;
        }

        /// <summary>
        /// The FindByPasswordToken.
        /// </summary>
        /// <param name="token">The token<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserDto}"/>.</returns>
        public async Task<UserDto> FindByPasswordToken(Guid token)
        {
            User user = await this.userRepository.FindByPasswordToken(token).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }
            return user.PerformMapping<User, UserDto>();
        }
    }
}
