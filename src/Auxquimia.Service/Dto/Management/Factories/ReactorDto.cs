namespace Auxquimia.Dto.Management.Factories
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ReactorDto" />.
    /// </summary>
    [Serializable]
    public class ReactorDto
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
        /// Gets or sets the DbRead.
        /// </summary>
        [Required]
        public int DbRead { get; set; }

        /// <summary>
        /// Gets or sets the DbWrite.
        /// </summary>
        [Required]
        public int DbWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
