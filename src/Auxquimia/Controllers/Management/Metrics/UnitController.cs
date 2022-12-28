namespace Auxquimia.Controllers.Management.Metrics
{
    using Auxquimia.Dto.Management.Metrics;
    using Auxquimia.Filters;
    using Auxquimia.Service.Management.Metrics;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UnitController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UnitController : Controller
    {
        /// <summary>
        /// Defines the unitService.
        /// </summary>
        private readonly IUnitService unitService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<UnitController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitController"/> class.
        /// </summary>
        /// <param name="unitService">The unitService<see cref="IUnitService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{UnitController}"/>.</param>
        public UnitController(IUnitService unitService, ILogger<UnitController> logger)
        {
            this.unitService = unitService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<UnitDto>))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<UnitDto> results = await unitService.PaginatedAsync(new FindRequestDto<BaseSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="unitId">The unitId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{unitId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UnitDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid unitId)
        {
            UnitDto unit = await unitService.GetAsync(unitId);

            if (unit == null)
            {
                return NotFound();
            }

            return Ok(unit);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="unit">The unit<see cref="UnitDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UnitDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] UnitDto unit)
        {
            if (unit.Id != default(string))
            {
                return BadRequest();
            }
            await unitService.SaveAsync(unit);
            return Ok(unit);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="unit">The unit<see cref="UnitDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UnitDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] UnitDto unit)
        {
            if (unit.Id == default(string))
            {
                return BadRequest();
            }
            await unitService.UpdateAsync(unit);
            return Ok(unit);
        }
    }
}
