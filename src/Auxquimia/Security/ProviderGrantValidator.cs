namespace Auxquimia.Security
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Service.Authentication;
    using Auxquimia.Utils;
    using IdentityServer4.Models;
    using IdentityServer4.Validation;
    using System;
    using System.Threading.Tasks;

    public class ProviderGrantValidator : IExtensionGrantValidator
    {
        private readonly IUserService userService;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ProviderGrantValidator(IUserService userService)
        {
            this.userService = userService;
        }

        public string GrantType => "urlToken";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                string urlToken = context.Request.Raw.Get("providerToken");
                UserDto user = null;
                if (urlToken != null)
                {
                    Guid passworId = urlToken.PerformMapping<string, Guid>();
                    user = await this.userService.FindByPasswordToken(passworId).ConfigureAwait(false);
                }
                if(user != null)
                {
                    context.Result = new GrantValidationResult(
                          subject: user.Username,
                          authenticationMethod: "providerToken");
                }
                else
                {
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant,
                        "Invalid token");
                }
                //context.Result = user != null ?
                //    new GrantValidationResult(
                //          subject: user.Id,
                //          authenticationMethod: "providerToken") :
                //    new GrantValidationResult(
                //        TokenRequestErrors.InvalidGrant,
                //        "Invalid token");
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
