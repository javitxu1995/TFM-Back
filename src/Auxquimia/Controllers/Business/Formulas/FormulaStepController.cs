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
    /// Defines the <see cref="FormulaStepController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormulaStepController : ControllerBase
    {
        /// <summary>
        /// Defines the formulaStepService.
        /// </summary>
        private readonly IFormulaStepService formulaStepService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<FormulaStepController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaStepController"/> class.
        /// </summary>
        /// <param name="formulaStepService">The formulaStepService<see cref="IFormulaStepService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{FormulaStepController}"/>.</param>
        public FormulaStepController(IFormulaStepService formulaStepService, ILogger<FormulaStepController> logger)
        {
            this.formulaStepService = formulaStepService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<FormulaStepDto>))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<FormulaStepDto> results = await formulaStepService.PaginatedAsync(new FindRequestDto<BaseSearchFilter>
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid formulaStepId)
        {
            FormulaStepDto formulaStep = await formulaStepService.GetAsync(formulaStepId);

            if (formulaStep == null)
            {
                return NotFound();
            }

            return Ok(formulaStep);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="formulaStep">The formulaStep<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] FormulaStepDto formulaStep)
        {
            if (formulaStep.Id != default(string))
            {
                return BadRequest();
            }
            await formulaStepService.SaveAsync(formulaStep);
            return Ok(formulaStep);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="formulaStep">The formulaStep<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaStepDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] FormulaStepDto formulaStep)
        {
            if (formulaStep.Id == default(string))
            {
                return BadRequest();
            }
            await formulaStepService.UpdateAsync(formulaStep);
            return Ok(formulaStep);
        }
    }
}
