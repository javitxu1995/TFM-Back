namespace Auxquimia.Filters.Business.AssemblyBuilds
{
    using Auxquimia.Enums;
    using System;

    /// <summary>
    /// Defines the <see cref="BaseAssemblyBuildSearchFilter" />.
    /// </summary>
    public class BaseAssemblyBuildSearchFilter
    {
        /// <summary>
        /// Gets or sets the AssemblyBuildNumber.
        /// </summary>
        public string AssemblyBuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public ABStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the MultipleStatusQuery.
        /// </summary>
        public ABStatus[] MultipleStatusQuery { get; set; }

        /// <summary>
        /// Gets or sets the FactoryId.
        /// </summary>
        public Guid FactoryId { get; set; }

        /// <summary>
        /// Gets or sets the BlenderId.
        /// </summary>
        public Guid BlenderId { get; set; }

        /// <summary>
        /// Gets or sets the OperatorAssignedId.
        /// </summary>
        public Guid OperatorAssignedId { get; set; }
    }
}
