namespace Auxquimia.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Defines the <see cref="ValidateModelAttribute" />
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The OnActionExecuting
        /// </summary>
        /// <param name="context">The context<see cref="ActionExecutingContext"/></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                logger.Error("model state is not valid at {0}", context.RouteData);
                context.Result = new JsonResult(context.ModelState)
                {
                    StatusCode = 400
                };
            }
        }
    }
}
