﻿namespace Auxquimia.Controllers.Management.Factories
{
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Service.Management.Factories;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReactorController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReactorController : Controller
    {
        /// <summary>
        /// Defines the reactorService.
        /// </summary>
        private readonly IReactorService reactorService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<ReactorController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorController"/> class.
        /// </summary>
        /// <param name="reactorService">The reactorService<see cref="IReactorService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{ReactorController}"/>.</param>
        public ReactorController(IReactorService reactorService, ILogger<ReactorController> logger)
        {
            this.reactorService = reactorService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="ReactorSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<ReactorDto>))]
        public async Task<IActionResult> Search([FromQuery] ReactorSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }
            Page<ReactorDto> results = await reactorService.PaginatedAsync(new FindRequestDto<ReactorSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{reactorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ReactorDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid reactorId)
        {
            ReactorDto reactor = await reactorService.GetAsync(reactorId);

            if (reactor == null)
            {
                return NotFound();
            }

            return Ok(reactor);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="reactor">The reactor<see cref="ReactorDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ReactorDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] ReactorDto reactor)
        {
            if (reactor.Id != default(string))
            {
                return BadRequest();
            }
            await reactorService.SaveAsync(reactor);
            return Ok(reactor);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="reactor">The reactor<see cref="ReactorDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ReactorDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] ReactorDto reactor)
        {
            if (reactor.Id == default(string))
            {
                return BadRequest();
            }
            await reactorService.UpdateAsync(reactor);
            return Ok(reactor);
        }
    }
}
