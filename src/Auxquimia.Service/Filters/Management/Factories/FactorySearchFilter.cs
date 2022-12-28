using System;

namespace Auxquimia.Filters.Management.Factories
{
    /// <summary>
    /// Defines the <see cref="FactorySearchFilter" />.
    /// </summary>
    public class FactorySearchFilter : BaseSearchFilter
    {
        /// <summary>
        /// Gets or sets the CountryId.
        /// </summary>
        public Guid CountryId { get; set; }
    }
}
