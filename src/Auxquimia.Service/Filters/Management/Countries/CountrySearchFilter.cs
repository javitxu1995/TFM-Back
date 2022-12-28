using Auxquimia.Filters;

namespace Auxquimia.Service.Filters.Management.Countries
{
    /// <summary>
    /// Search filter for country
    /// </summary>
    public class CountrySearchFilter: BaseSearchFilter
    {
        /// <summary>
        ///  Gets or sets the iso name
        /// </summary>
        public string IsoName { get; set; }
    }
}
