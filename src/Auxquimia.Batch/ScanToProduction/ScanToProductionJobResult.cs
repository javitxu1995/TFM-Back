using Auxquimia.Batch.Infraestructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Batch.ScanToProduction
{
    public class ScanToProductionJobResult : JobResult
    {
        /// <summary>
        /// Defines the TEMPLATE.
        /// </summary>
        private static readonly string TEMPLATE = @"templates/scan/text.cshtml";

        /// <summary>
        /// Defines the TEMPLATE_HTML.
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/scan/html.cshtml";

        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteReadJobResult"/> class.
        /// </summary>
        public ScanToProductionJobResult()
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
