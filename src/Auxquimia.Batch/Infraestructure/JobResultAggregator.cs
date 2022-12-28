using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Auxquimia.Batch.Infraestructure.JobResult" />
    public class JobResultAggregator : JobResult
    {
        /** The Constant TEMPLATE. */

        /// <summary>
        /// The template
        /// </summary>
        private static readonly string TEMPLATE = @"templates/aggregator/text.cshtml";

        /** The Constant TEMPLATE_HTML. */

        /// <summary>
        /// The template HTML
        /// </summary>
        private static readonly string TEMPLATE_HTML = @"templates/aggregator/html.cshtml";

        /// <summary>
        /// The job results
        /// </summary>
        private IList<JobResult> jobResults = new List<JobResult>();

        /// <summary>
        /// The is HTML
        /// </summary>
        private bool isHtml;

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public string Results
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                foreach (JobResult result in jobResults)
                {
                    if (isHtml)
                    {
                        builder.Append(result.GetReportHtml().GetAwaiter().GetResult());
                        builder.Append(StringPool.HTML_LINE_SEPARATOR);
                    }
                    else
                    {
                        builder.Append(result.GetReport().GetAwaiter().GetResult());
                        builder.Append(Environment.NewLine);
                    }
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the stopwatch.
        /// </summary>
        /// <value>
        /// The stopwatch.
        /// </value>
        public string Stopwatch
        {
            get
            {
                return Infraestructure.Stopwatch.GetInstance().GenerateReport();
            }
        }

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <returns></returns>
        public override Task<string> GetReport()
        {
            isHtml = false;
            return base.GetReport();
        }

        /// <summary>
        /// Gets the report HTML.
        /// </summary>
        /// <returns></returns>
        public override Task<string> GetReportHtml()
        {
            isHtml = true;
            return base.GetReportHtml();
        }

        /// <summary>
        /// Appends the specified job result.
        /// </summary>
        /// <param name="jobResult">The job result.</param>
        public void Append(JobResult jobResult)
        {
            jobResults.Add(jobResult);
        }

        /// <summary>
        /// Gets the HTML template path.
        /// </summary>
        /// <returns></returns>
        public override string GetHtmlTemplatePath()
        {
            return TEMPLATE_HTML;
        }

        /// <summary>
        /// Gets the template path.
        /// </summary>
        /// <returns></returns>
        public override string GetTemplatePath()
        {
            return TEMPLATE;
        }
    }
}
