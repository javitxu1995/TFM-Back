namespace Auxquimia.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using JwtBearerDefaults = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

    /// <summary>
    /// Defines the <see cref="IdentityController" />
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// The Get
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
