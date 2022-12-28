namespace Auxquimia.Controllers.Authentication
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Filters;
    using Auxquimia.Service.Authentication;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using JwtBearerDefaults = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

    /// <summary>
    /// Controller for user related operations.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The user service.......
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The userService<see cref="IUserService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{UserController}"/>.</param>
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="UserSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<UserDto>))]
        public async Task<IActionResult> Search([FromQuery] UserSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<UserDto> results = await userService.PaginatedAsync(new FindRequestDto<UserSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="UserSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("searchForSelect")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<UserDto>))]
        public async Task<IActionResult> SearchForSelect([FromQuery] UserSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<UserDto> results = await userService.SearchForSelect(new FindRequestDto<UserSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The SearhHighUsers.
        /// </summary>
        /// <param name="filter">The filter<see cref="UserSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("searchHighUsers")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<UserDto>))]
        public async Task<IActionResult> SearchHighUsers([FromQuery] UserSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<UserDto> results = await userService.SearchHighUsers(new FindRequestDto<UserSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// Retrieves the detail for a single user given its id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The user.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid userId)
        {
            UserDto user = await userService.GetAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Saves a user to the database.
        /// </summary>
        /// <param name="user">the user to be saved.</param>
        /// <returns>the same user updated once saved (encrypted password + id).</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] UserDto user)
        {
            if (user.Id != default(string))
            {
                return BadRequest();
            }
            //await userService.SaveAsync(user);
            UserDto userSaved = await this.userService.SaveAsync(user);
            return Ok(userSaved);
        }

        /// <summary>
        /// Updates a user information in the database.
        /// </summary>
        /// <param name="user">the user to be updated.</param>
        /// <returns>the user updated once saved (encrypted password if changed).</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] UserDto user)
        {
            if (user.Id == default(string))
            {
                return BadRequest();
            }
            //await userService.UpdateAsync(user);
            UserDto userUpdated = await this.userService.UpdateAsync(user);
            return Ok(userUpdated);
        }

        /// <summary>
        /// Enables a user.
        /// </summary>
        /// <param name="userId">.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpPut("{userId}/enable")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> EnableUser(Guid userId)
        {
            await userService.ToggleEnabledUserAsync(userId, true);
            return NoContent();
        }

        /// <summary>
        /// Disables a user.
        /// </summary>
        /// <param name="userId">.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpPut("{userId}/disable")]
        public async Task<IActionResult> DisableUser(Guid userId)
        {
            await userService.ToggleEnabledUserAsync(userId, false);
            return NoContent();
        }

        /// <summary>
        /// The GetByEmail.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            UserDto user = await userService.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// The ResetUsePassword.
        /// </summary>
        /// <param name="userId">The userId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("password/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ResetUsePassword(Guid userId)
        {
            if (userId == default(Guid))
            {
                return BadRequest();
            }

            bool result = await userService.ResetPasswordForUser(userId);
            return Ok(result);
        }

        /// <summary>
        /// The UpdateUsePassword.
        /// </summary>
        /// <param name="userData">The userData<see cref="UserPassDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("password")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUsePassword([FromBody] UserPassDto userData)
        {
            if (userData == null)
            {
                return BadRequest();
            }

            bool result = await userService.UpdatePasswordForUser(userData.UserId.PerformMapping<string, Guid>(), userData.Password);
            return Ok(result);
        }

        /// <summary>
        /// The GetUserByPasswordToken.
        /// </summary>
        /// <param name="passwordToken">The passwordToken<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("password/token/{passwordToken}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserByPasswordToken(Guid passwordToken)
        {
            if (passwordToken == default(Guid))
            {
                return BadRequest();
            }
            UserDto user = await userService.FindByPasswordToken(passwordToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(true);
        }
    }
}
