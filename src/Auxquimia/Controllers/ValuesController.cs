namespace Auxquimia.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Auxquimia.Service.Authentication;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ValuesController" />
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Defines the configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Defines the logger
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesController"/> class.
        /// </summary>
        /// <param name="userService">The userService<see cref="IUserService"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public ValuesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// The Get
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult{IEnumerable{string}}}"/></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            logger.Info("Called method GET");
            return new string[] { "value1", "value2", this.configuration.GetValue<string>("section0:var1"), this.configuration.GetValue<string>("section0:var2") };
        }

        /// <summary>
        /// The Get
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ActionResult{string}"/></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// The Post
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            logger.Info($"Called method POST with value: [{value}]");
        }

        /// <summary>
        /// The Put
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            logger.Info($"Called method PUT with value: [{value}]");
        }

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            logger.Info($"Called method DELETE with id: [{id}]");
        }
    }
}
