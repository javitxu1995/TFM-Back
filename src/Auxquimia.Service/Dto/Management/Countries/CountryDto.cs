namespace Auxquimia.Dto.Management.Countries
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The DTO for a Country
    /// </summary>
    [Serializable]
    public class CountryDto
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the iso name
        /// </summary>
        [Required]
        public string IsoName { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }

}
