namespace Auxquimia.Controllers.Management.Factories
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
    /// Defines the <see cref="FactoryController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FactoryController : Controller
    {
        /// <summary>
        /// Defines the factoryService.
        /// </summary>
        private readonly IFactoryService factoryService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<FactoryController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryController"/> class.
        /// </summary>
        /// <param name="factoryService">The factoryService<see cref="IFactoryService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{FactoryController}"/>.</param>
        public FactoryController(IFactoryService factoryService, ILogger<FactoryController> logger)
        {
            this.factoryService = factoryService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="FactorySearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<FactoryListDto>))]
        public async Task<IActionResult> Search([FromQuery] FactorySearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<FactoryListDto> results = await factoryService.PaginatedAsync(new FindRequestDto<FactorySearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{factoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FactoryDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid factoryId)
        {
            FactoryDto factory = await factoryService.GetAsync(factoryId);

            if (factory == null)
            {
                return NotFound();
            }

            return Ok(factory);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="factory">The factory<see cref="FactoryDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FactoryDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] FactoryDto factory)
        {
            if (factory.Id != default(string))
            {
                return BadRequest();
            }
            await factoryService.SaveAsync(factory);
            return Ok(factory);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="factory">The factory<see cref="FactoryDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FactoryDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] FactoryDto factory)
        {
            if (factory.Id == default(string))
            {
                return BadRequest();
            }
            await factoryService.UpdateAsync(factory);
            return Ok(factory);
        }
    }
}
