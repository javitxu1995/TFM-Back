namespace Auxquimia
{
    using IdentityModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Service.Authentication;
    using System.Linq;
    using System.Threading.Tasks;
    using JwtBearerDefaults = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

    /// <summary>
    /// Defines the <see cref="CurrentUserController" />
    /// </summary>
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CurrentUserController : ControllerBase
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserController"/> class.
        /// </summary>
        /// <param name="userService">The userService<see cref="IUserService"/></param>
        public CurrentUserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// The CurrentUserData
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet]
        public async Task<IActionResult> CurrentUserData()
        {
            string username = User.Claims.First(c => c.Type == JwtClaimTypes.Subject)?.Value;
            UserDto result = null;

            if (!string.IsNullOrWhiteSpace(username))
            {
                result = await userService.FindByUsernameAsync(username);
            }

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
