namespace Auxquimia.Filters
{
    using Auxquimia.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ApiExceptionFilter" />
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The OnException
        /// </summary>
        /// <param name="context">The context<see cref="ExceptionContext"/></param>
        public override void OnException(ExceptionContext context)
        {
           ApiError apiError = null;
            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;

                // handle logging here
            }
            else if (context.Exception is CustomException ex)
            {
                apiError = new ApiError(ex.Message);
                context.HttpContext.Response.StatusCode = 400; // Bad Request

                // handle logging here
            }
            else
            {
                logger.Error(context.Exception, "An unhandled exception was captured: {0}", context.Exception.Message);
                // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError(msg);
                apiError.Detail = stack;

                context.HttpContext.Response.StatusCode = 500;

                // handle logging here
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }

    /// <summary>
    /// Defines the <see cref="ApiException" />
    /// </summary>
    [Serializable]
    public class ApiException : System.Exception
    {
        /// <summary>
        /// Gets or sets the StatusCode
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="statusCode">The statusCode<see cref="int"/></param>
        public ApiException(string message,
                        int statusCode = 500) :
        base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="System.Exception"/></param>
        /// <param name="statusCode">The statusCode<see cref="int"/></param>
        public ApiException(System.Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Defines the <see cref="ApiError" />
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsError
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets the Detail
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public ApiError(string message)
        {
            Message = message;
            IsError = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class.
        /// </summary>
        /// <param name="modelState">The modelState<see cref="ModelStateDictionary"/></param>
        public ApiError(ModelStateDictionary modelState)
        {
            this.IsError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                Message = "Please correct the specified errors and try again.";
            }
        }
    }
}
