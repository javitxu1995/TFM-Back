namespace Auxquimia.Controllers.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Auxquimia.Service.Business.Formulas;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStepController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NetsuiteFormulaStepController : ControllerBase
    {
        /// <summary>
        /// Defines the netsuiteFormulaStepService.
        /// </summary>
        private readonly INetsuiteFormulaStepService netsuiteFormulaStepService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<NetsuiteFormulaStepController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaStepController"/> class.
        /// </summary>
        /// <param name="netsuiteFormulaStepService">The netsuiteFormulaStepService<see cref="INetsuiteFormulaStepService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{NetsuiteFormulaStepController}"/>.</param>
        public NetsuiteFormulaStepController(INetsuiteFormulaStepService netsuiteFormulaStepService, ILogger<NetsuiteFormulaStepController> logger)
        {
            this.netsuiteFormulaStepService = netsuiteFormulaStepService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<NetsuiteFormulaStepDto>))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<NetsuiteFormulaStepDto> results = await netsuiteFormulaStepService.PaginatedAsync(new FindRequestDto<BaseSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="formulaStepId">The formulaStepId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{formulaStepId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NetsuiteFormulaStepDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid formulaStepId)
        {
            NetsuiteFormulaStepDto formulaStep = await netsuiteFormulaStepService.GetAsync(formulaStepId);

            if (formulaStep == null)
            {
                return NotFound();
            }

            return Ok(formulaStep);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="formulaStep">The formulaStep<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NetsuiteFormulaStepDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] NetsuiteFormulaStepDto formulaStep)
        {
            if (formulaStep.Id != default(string))
            {
                return BadRequest();
            }
            await netsuiteFormulaStepService.SaveAsync(formulaStep);
            return Ok(formulaStep);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="formulaStep">The formulaStep<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NetsuiteFormulaStepDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] NetsuiteFormulaStepDto formulaStep)
        {
            if (formulaStep.Id == default(string))
            {
                return BadRequest();
            }
            await netsuiteFormulaStepService.UpdateAsync(formulaStep);
            return Ok(formulaStep);
        }
    }
}
