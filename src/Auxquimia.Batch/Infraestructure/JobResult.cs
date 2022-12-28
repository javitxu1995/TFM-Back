using System;
using System.IO;
using System.Threading.Tasks;
using RazorLight;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    public abstract class JobResult
    {
        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> GetReport()
        {
            return ProcessTemplate(GetTemplatePath());
        }

        /// <summary>
        /// Gets the report HTML.
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> GetReportHtml()
        {
            return ProcessTemplate(GetHtmlTemplatePath());
        }

        /// <summary>
        /// Processes the template.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private async Task<string> ProcessTemplate(string path)
        {
            var engine = new RazorLightEngineBuilder()
              .UseMemoryCachingProvider()
              .Build();

            string template = await System.IO.File.ReadAllTextAsync(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), path));

            return await engine.CompileRenderAsync(GetType().Name, template, this).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the template path.
        /// </summary>
        /// <returns></returns>
        public abstract string GetTemplatePath();

        /// <summary>
        /// Gets the HTML template path.
        /// </summary>
        /// <returns></returns>
        public abstract string GetHtmlTemplatePath();
    }
}
