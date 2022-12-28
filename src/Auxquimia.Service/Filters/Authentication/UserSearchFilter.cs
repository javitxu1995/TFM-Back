namespace Auxquimia.Service.Filters.Authentication
{
    using System;

    /// <summary>
    /// Search filter for users.
    /// </summary>
    public class UserSearchFilter
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
