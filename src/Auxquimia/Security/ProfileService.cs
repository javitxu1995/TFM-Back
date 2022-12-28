namespace Auxquimia.Security
{
    using IdentityModel;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Service.Authentication;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ProfileService" />
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// Defines the userService
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="userService">The userService<see cref="IUserService"/></param>
        public ProfileService(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// The GetProfileDataAsync
        /// </summary>
        /// <param name="context">The context<see cref="ProfileDataRequestContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string username = context.Subject?.FindFirstValue(JwtClaimTypes.Subject);

            if (!string.IsNullOrWhiteSpace(username))
            {
                UserDto user = await userService.FindByUsernameAsync(username);
                var previousClaims = context.Subject.FindAll(JwtClaimTypes.Role);
                List<Claim> resultingClaims = new List<Claim>();
                resultingClaims.AddRange(previousClaims);

                foreach (RoleDto role in user.Roles)
                {
                    resultingClaims.Add(new Claim(JwtClaimTypes.Role, role.Name));
                }

                context.IssuedClaims.AddRange(resultingClaims);
            }
        }

        /// <summary>
        /// The IsActiveAsync
        /// </summary>
        /// <param name="context">The context<see cref="IsActiveContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;
            string username = context.Subject?.FindFirstValue(JwtClaimTypes.Subject);

            if (!string.IsNullOrWhiteSpace(username))
            {
                UserDto user = await userService.FindByUsernameAsync(username);
                context.IsActive = user != null && user.Valid;
            }
        }
    }
}
