namespace Auxquimia.Dto.Management.Factories
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="FactoryListDto" />.
    /// </summary>
    [Serializable]
    public class FactoryListDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the OpcServer.
        /// </summary>
        [Required]
        public string OpcServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        [Required]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NoManagers.
        /// </summary>
        public bool NoManagers { get; set; }
    }
}
