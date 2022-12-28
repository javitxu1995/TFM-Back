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
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FormulaController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormulaController : Controller
    {
        /// <summary>
        /// Defines the formulaService.
        /// </summary>
        private readonly IFormulaService formulaService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<FormulaController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaController"/> class.
        /// </summary>
        /// <param name="formulaService">The formulaService<see cref="IFormulaService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{FormulaController}"/>.</param>
        public FormulaController(IFormulaService formulaService, ILogger<FormulaController> logger)
        {
            this.formulaService = formulaService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<FormulaDto>))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<FormulaDto> results = await formulaService.PaginatedAsync(new FindRequestDto<BaseSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        [HttpGet("search/forassembly")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<FormulaDto>))]
        public async Task<IActionResult> SearchForAssembly([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<FormulaDto> results = await formulaService.GetForAssembly(new FindRequestDto<BaseSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        [HttpGet("search/notOnProduction")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<FormulaDto>))]
        public async Task<IActionResult> SearchNotOnProduction([FromQuery] BaseSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<FormulaDto> results = await formulaService.FindNotOnProduction(new FindRequestDto<BaseSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="formulaId">The formulaId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{formulaId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid formulaId)
        {
            FormulaDto formula = await formulaService.GetAsync(formulaId);

            if (formula == null)
            {
                return NotFound();
            }

            return Ok(formula);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="formula">The formula<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] FormulaDto formula)
        {
            if (formula.Id != default(string))
            {
                return BadRequest();
            }
            await formulaService.SaveAsync(formula);
            return Ok(formula);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="formula">The formula<see cref="FormulaDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] FormulaDto formula)
        {
            if (formula.Id == default(string))
            {
                return BadRequest();
            }
            await formulaService.UpdateAsync(formula);
            return Ok(formula);
        }


    }
}
