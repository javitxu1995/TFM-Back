namespace Auxquimia.Service.Filters.Authentication
{
    using Auxquimia.Filters;
    using System;

    /// <summary>
    /// Search filter for users.
    /// </summary>
    public class UserSearchFilter : ISearchFilter
    {
        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Enabled.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets the facotryId.
        /// </summary>
        public Guid? FactoryId { get; set; }

    }
}
