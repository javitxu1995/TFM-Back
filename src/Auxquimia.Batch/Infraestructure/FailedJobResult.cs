using System;
namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Auxquimia.Batch.Infraestructure.JobResult" />
    public class FailedJobResult : JobResult
    {
        /** The Constant TEMPLATE. */

        /// <summary>
        /// The template
        /// </summary>
        private static readonly string TEMPLATE = @"templates/failed/text.cshtml";

        /** The Constant TEMPLATE_HTML. */

        /// <summary>
        /// The template HTML
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/failed/html.cshtml";

        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the stack tracke.
        /// </summary>
        /// <value>
        /// The stack tracke.
        /// </value>
        public string StackTracke
        {
            get
            {
                return Exception.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// * Gets the html template path.
        /// *
        /// * @return the html template path
        /// */
        public override string GetHtmlTemplatePath()
        {
            return TEMPLATE;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// * Gets the template path.
        /// *
        /// * @return the template path
        /// */
        public override string GetTemplatePath()
        {
            return TEMPLATE_HTML;
        }
    }
}
