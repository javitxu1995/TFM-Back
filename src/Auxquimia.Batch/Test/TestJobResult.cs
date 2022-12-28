namespace Auxquimia.Batch.Test
{
    using Auxquimia.Batch.Infraestructure;

    /// <summary>
    /// Defines the <see cref="TestJobResult" />.
    /// </summary>
    public class TestJobResult : JobResult
    {
        /// <summary>
        /// The template.
        /// </summary>
        private static readonly string TEMPLATE = @"templates/test/text.cshtml";

        /// <summary>
        /// The template HTML.
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/test/html.cshtml";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestJobResult"/> class.
        /// </summary>
        public TestJobResult()
        {
        }

        /// <summary>
        /// The GetHtmlTemplatePath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string GetHtmlTemplatePath()
        {
            return TEMPLATE_HTML;
        }

        /// <summary>
        /// The GetTemplatePath.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string GetTemplatePath()
        {
            return TEMPLATE;
        }
    }
}
