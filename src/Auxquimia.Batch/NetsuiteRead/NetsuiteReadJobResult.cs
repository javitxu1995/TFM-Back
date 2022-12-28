namespace Auxquimia.Batch.NetsuiteRead
{
    using Auxquimia.Batch.Infraestructure;

    /// <summary>
    /// Defines the <see cref="NetsuiteReadJobResult" />.
    /// </summary>
    public class NetsuiteReadJobResult : JobResult
    {
        /// <summary>
        /// Defines the TEMPLATE.
        /// </summary>
        private static readonly string TEMPLATE = @"templates/ftp/text.cshtml";

        /// <summary>
        /// Defines the TEMPLATE_HTML.
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/ftp/html.cshtml";

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteReadJobResult"/> class.
        /// </summary>
        public NetsuiteReadJobResult()
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
