namespace Auxquimia.Filters
{
    /// <summary>
    /// Defines the <see cref="BaseSearchFilter" />.
    /// </summary>
    public class BaseSearchFilter : ISearchFilter
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public string Code { get; set; }
    }
}
