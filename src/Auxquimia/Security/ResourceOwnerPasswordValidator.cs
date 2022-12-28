namespace Auxquimia.Security
{
    using IdentityServer4.Models;
    using IdentityServer4.Validation;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Service.Authentication;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ResourceOwnerPasswordValidator" />
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// Defines the userService
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Defines the logger
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="userService">The userService<see cref="IUserService"/></param>
        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// The ValidateAsync
        /// </summary>
        /// <param name="context">The context<see cref="ResourceOwnerPasswordValidationContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                UserDto user = await userService.FindByUsernameAndPasswordAsync(context.UserName, context.Password);

                context.Result = user != null && user.Valid ?
                    new GrantValidationResult(
                          subject: user.Username,
                          authenticationMethod: "custom") :
                    new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant,
                        "Invalid username or password");
            }
            catch (System.Exception e)
            {
                logger.Error(e, e.Message);
                context.Result = new GrantValidationResult(
                      TokenRequestErrors.InvalidGrant,
                      "Internal error");
            }
        }
    }
}
