namespace Auxquimia.Controllers.Management.Business.AssemblyBuilds
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Enums;
    using Auxquimia.Filters;
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Service.Business.AssemblyBuilds;
    using Auxquimia.Service.Business.Kafka;
    using Auxquimia.Service.Management.Factories;
    using IdentityModel;
    using Izertis.Paging.Abstractions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildController" />.
    /// </summary>
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssemblyBuildController : Controller
    {
        /// <summary>
        /// Defines the assemblyBuildService.
        /// </summary>
        private readonly IAssemblyBuildService assemblyBuildService;

        /// <summary>
        /// Defines the auxquimiaKafkaService.
        /// </summary>
        private readonly IAuxquimiaKafkaService auxquimiaKafkaService;

        /// <summary>
        /// Defines the reactorService.
        /// </summary>
        private readonly IReactorService reactorService;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<AssemblyBuildController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildController"/> class.
        /// </summary>
        /// <param name="assemblyBuildService">The assemblyBuildService<see cref="IAssemblyBuildService"/>.</param>
        /// <param name="auxquimiaKafkaService">The auxquimiaKafkaService<see cref="IAuxquimiaKafkaService"/>.</param>
        /// <param name="reactorService">The reactorService<see cref="IReactorService"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{AssemblyBuildController}"/>.</param>
        public AssemblyBuildController(IAssemblyBuildService assemblyBuildService,
            IAuxquimiaKafkaService auxquimiaKafkaService,
            IReactorService reactorService,
            ILogger<AssemblyBuildController> logger)
        {
            this.assemblyBuildService = assemblyBuildService;
            this.auxquimiaKafkaService = auxquimiaKafkaService;
            this.reactorService = reactorService;
            this.logger = logger;
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<AssemblyBuildListDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Search([FromQuery] BaseAssemblyBuildSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching with params {filter} and {pageRequest}");
            }
            string username = User.Claims.First(c => c.Type == JwtClaimTypes.Subject)?.Value;
            if (username == null || username == default(string))
            {
                return BadRequest();
            }
            Page<AssemblyBuildListDto> results = await assemblyBuildService.SearchByRole(username, new FindRequestDto<BaseAssemblyBuildSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The UpdateOperator.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="operatorId">The operatorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("updateOperator/{assemblyId}/{operatorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        public async Task<IActionResult> UpdateOperator(Guid assemblyId, Guid operatorId)
        {
            AssemblyBuildDto result = await assemblyBuildService.UpdateOperatorAsync(assemblyId, operatorId).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{assemblyBuildId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid assemblyBuildId)
        {
            AssemblyBuildDto assemblyBuild = await assemblyBuildService.GetAsync(assemblyBuildId);

            if (assemblyBuild == null)
            {
                return NotFound();
            }

            return Ok(assemblyBuild);
        }

        /// <summary>
        /// The Save.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Save([FromBody] AssemblyBuildDto assemblyBuild)
        {
            if (assemblyBuild.Id != default(string))
            {
                return BadRequest();
            }
            await assemblyBuildService.SaveAsync(assemblyBuild);
            return Ok(assemblyBuild);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] AssemblyBuildDto assemblyBuild)
        {
            if (assemblyBuild.Id == default(string))
            {
                return BadRequest();
            }
            await assemblyBuildService.UpdateAsync(assemblyBuild);
            return Ok(assemblyBuild);
        }

        /// <summary>
        /// The CountByStatus.
        /// </summary>
        /// <param name="filter">The filter<see cref="AssemblyFactoryDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("status-count")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Dictionary<ABStatus, int>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CountByStatus([FromBody] AssemblyFactoryDto filter)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Counting Assembly Builds by status.");
            }

            string username = User.Claims.First(c => c.Type == JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }
            Guid factoryId = default(Guid);
            if (filter != null)
            {
                factoryId = filter.FactoryId;
            }

            Dictionary<ABStatus, int> result = await assemblyBuildService.GetCountWoByStatus(username, factoryId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// The Search.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequestDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("status")]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Page<AssemblyBuildListDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> FilterByMultipleStatus([FromBody] BaseAssemblyBuildSearchFilter filter, [FromQuery] PageRequestDto pageRequest)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Searching by multiple status with params {filter} and {pageRequest}");
            }
            string username = User.Claims.First(c => c.Type == JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            Page<AssemblyBuildListDto> results = await assemblyBuildService.GetByMultipleStatus(username, new FindRequestDto<BaseAssemblyBuildSearchFilter>
            {
                Filter = filter,
                PageRequest = pageRequest
            });
            return Ok(results);
        }

        /// <summary>
        /// The testReadFromFtp.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("test/ftp/read")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IList<AssemblyBuildDto>))]
        public async Task<IActionResult> testReadFromFtp()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Reading Assembly Builds from FTP server. TEST");
            }

            IList<AssemblyBuildDto> results = await assemblyBuildService.LoadFromFtp();

            return Ok(results);
        }

        /// <summary>
        /// The UpdateKafkaListener.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("test/ftp/write")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildListDto))]
        public async Task<IActionResult> WriteOnNetsuite()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Writing Assembly Builds to FTP server. TEST");
            }
            IList<AssemblyBuildListDto> assemblies = await assemblyBuildService.SendToNetsuite();

            return Ok(assemblies);
        }

        /// <summary>
        /// The WriteAssemblyToKafka.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("test/kafka/write/{assemblyBuildId}")]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FormulaDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> WriteAssemblyToKafka(Guid assemblyBuildId)
        {
            if (assemblyBuildId == default(Guid))
            {
                return BadRequest();
            }
            AssemblyBuildDto assembly = await auxquimiaKafkaService.SendAssemblyBuildToKafka(assemblyBuildId);
            return Ok(assembly);
        }

        /// <summary>
        /// The SendById.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("send/{assemblyBuildId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendById(Guid assemblyBuildId)
        {
            if (assemblyBuildId == default(Guid))
            {
                return NotFound();
            }
            AssemblyBuildDto assemblyBuild = await assemblyBuildService.SendAssemblyToWaitingQueue(assemblyBuildId);
            if (assemblyBuild == null)
            {
                return BadRequest();
            }
            return Ok(true);
        }

        /// <summary>
        /// The UpdateKafkaListener.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("Kafka/Update")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateKafkaListener()
        {
            bool result = await auxquimiaKafkaService.UpdateKafkaConsumerTopics();

            return Ok(result);
        }

        /// <summary>
        /// The SendAssemblyToWaitingQueue.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("Production/Send/{reactorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendAssemblyToProduction(Guid reactorId)
        {
            if (reactorId == default(Guid))
            {
                return BadRequest();
            }
            ReactorDto reactor = await this.reactorService.GetAsync(reactorId);
            if (reactor == null) { return NotFound(); }
            AssemblyBuildDto result = await auxquimiaKafkaService.SendAssemblyToProduction(reactorId);

            return Ok(result);
        }

        /// <summary>
        /// The SendAllToProduction.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("Production/Send/All")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IList<AssemblyBuildDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendAllToProduction()
        {
            IList<AssemblyBuildDto> result = await auxquimiaKafkaService.SendAssembliesToProduction();

            return Ok(result);
        }

        /// <summary>
        /// The SendAllToProduction.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("Production/toPending/{assemblyId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BackToPending(Guid assemblyId)
        {
            if (assemblyId == default(Guid))
            {
                return BadRequest();
            }
            string username = User.Claims.First(c => c.Type == JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }
            AssemblyBuildDto result = await assemblyBuildService.SendBackToProgress(username, assemblyId);

            return Ok(result);
        }

        /// <summary>
        /// The SendAllToProduction.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("test/assembly/opc/{assemblyId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssemblyBuildDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TestGetAssemblyOpc(Guid assemblyId)
        {
            if (assemblyId == default(Guid))
            {
                return BadRequest();
            }
            AssemblyBuildDto result = await this.assemblyBuildService.GetToProductionAsync(assemblyId);

            if (result == null) { return NotFound(); }

            return Ok(result);
        }

        /// <summary>
        /// The AbortAssemblyProduction.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("Production/Abort/{assemblyId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AbortAssemblyProduction(Guid assemblyId)
        {
            if (assemblyId == default(Guid))
            {
                return BadRequest();
            }
            bool result = await this.auxquimiaKafkaService.SendAbortOrder(assemblyId);

            return Ok(result);
        }

        /// <summary>
        /// The UpdateNetsuiteStep.
        /// </summary>
        /// <param name="step">The step<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("lot/update/netsuite")]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateNetsuiteStep([FromBody] NetsuiteFormulaStepDto step)
        {
            if (step == null || step.Id == null || step.Id == default(string))
            {
                return BadRequest();
            }
            bool result = false;
            result = await this.auxquimiaKafkaService.SaveAndSendNetsuiteLotNewLot(step).ConfigureAwait(false);

            return Ok(result);
        }

        /// <summary>
        /// The UpdateFormulaStep.
        /// </summary>
        /// <param name="step">The step<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("lot/update/formula")]
        [ValidateModel]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateFormulaStep([FromBody] FormulaStepDto step)
        {

            if (step == null || step.Id == null || step.Id == default(string))
            {
                return BadRequest();
            }
            bool result = false;
            result = await this.auxquimiaKafkaService.SaveAndSendFormulaLotNewLot(step).ConfigureAwait(false);

            return Ok(result);
        }
    }
}
