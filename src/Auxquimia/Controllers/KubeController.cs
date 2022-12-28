namespace ImqGestion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Controler for Kubernetes related operations
    /// </summary>
    [Route("[controller]")]
    public class KubeController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KubeController"/> class.
        /// </summary>
        public KubeController()
        {
        }

        /// <summary>
        /// Retrieves the list of active products
        /// </summary>
        /// <returns>The list of active products</returns>
        [HttpGet("health")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Health()
        {
            return Ok("UP");
        }
    }
}