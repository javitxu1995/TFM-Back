namespace Auxquimia.Controllers.Management.Countries
{
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Filters;
    using Auxquimia.Service.Filters.Management.Countries;
    using Auxquimia.Service.Management.Countries;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="CountryController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CountryController : Controller
    {
        /// <summary>
        /// The country service.
        /// </summary>
        private readonly ICountryService countryService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<CountryController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryController"/> class.
        /// </summary>
        /// <param name="countryService">.</param>
        /// <param name="logger">.</param>
        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            this.countryService = countryService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="CountrySearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<CountryDto>))]
        public async Task<IActionResult> Search([FromQuery] CountrySearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }

            Page<CountryDto> results = await countryService.PaginatedAsync(new FindRequestDto<CountrySearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// Retrieves the detail for a single country given its id.
        /// </summary>
        /// <param name="countryId">The countryId<see cref="Guid"/>.</param>
        /// <returns>The user.</returns>
        [HttpGet("{countryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CountryDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid countryId)
        {
            CountryDto user = await countryService.GetAsync(countryId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Saves a country to the database.
        /// </summary>
        /// <param name="country">the country to be saved.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CountryDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] CountryDto country)
        {
            if (country.Id != default(string))
            {
                return BadRequest();
            }
            await countryService.SaveAsync(country);
            return Ok(country);
        }

        /// <summary>
        /// Updates a country information in the database.
        /// </summary>
        /// <param name="country">The country<see cref="CountryDto"/>.</param>
        /// <returns>the country updated once saved.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CountryDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] CountryDto country)
        {
            if (country.Id == default(string))
            {
                return BadRequest();
            }
            await countryService.UpdateAsync(country);
            return Ok(country);
        }
    }
}
