namespace Auxquimia.Dto.Management.Metrics
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="UnitDto" />.
    /// </summary>
    [Serializable]
    public class UnitDto
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
    }
}
