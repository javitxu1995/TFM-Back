namespace Auxquimia.Controllers.Authentication
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Filters;
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Service.Authentication;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Threading.Tasks;
    using JwtBearerDefaults = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

    /// <summary>
    /// Defines the <see cref="RoleController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        /// <summary>
        /// Defines the roleService.
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<RoleController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="roleService">The roleService<see cref="IRoleService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{RoleController}"/>.</param>
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            this.roleService = roleService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="RoleSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<RoleDto>))]
        public async Task<IActionResult> Search([FromQuery] RoleSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<RoleDto> results = await roleService.PaginatedAsync(new FindRequestDto<RoleSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="role">The role<see cref="RoleDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RoleDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] RoleDto role)
        {
            if (role.Id != default(string))
            {
                return BadRequest();
            }
            await roleService.SaveAsync(role);
            return Ok(role);
        }
    }
}
