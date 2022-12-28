using System;
using Auxquimia.Batch.Infraestructure;

namespace Auxquimia.Batch.TestMultistep
{
    public class MultistepJobResult : JobResult
    {
        /// <summary>
        /// The template
        /// </summary>
        private static readonly string TEMPLATE = @"templates/test/text.cshtml";

        /** The Constant TEMPLATE_HTML. */

        /// <summary>
        /// The template HTML
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/test/html.cshtml";

        public MultistepJobResult()
        {
        }

        public override string GetHtmlTemplatePath()
        {
            return TEMPLATE_HTML;
        }

        public override string GetTemplatePath()
        {
            return TEMPLATE;
        }
    }
}
