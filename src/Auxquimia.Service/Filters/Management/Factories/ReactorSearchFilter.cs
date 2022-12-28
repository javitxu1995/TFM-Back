namespace Auxquimia.Filters.Management.Factories
{
    using System;

    /// <summary>
    /// Defines the <see cref="ReactorSearchFilter" />.
    /// </summary>
    public class ReactorSearchFilter : BaseSearchFilter
    {
        /// <summary>
        /// Gets or sets the FactoryId.
        /// </summary>
        public Guid FactoryId { get; set; }
    }
}
