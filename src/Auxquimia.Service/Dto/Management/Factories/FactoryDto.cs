namespace Auxquimia.Dto.Management.Factories
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Management.Countries;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="FactoryDto" />.
    /// </summary>
    [Serializable]
    public class FactoryDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryDto"/> class.
        /// </summary>
        public FactoryDto()
        {
            this.Main = false;
        }

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
        public CountryDto Country { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        [Required]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the Reactors.
        /// </summary>
        public List<ReactorDto> Reactors { get; set; }

        /// <summary>
        /// Gets or sets the OpcServer.
        /// </summary>
        [Required]
        public string OpcServer { get; set; }

        /// <summary>
        /// Gets or sets the FactoryManagers.
        /// </summary>
        public List<UserDto> FactoryManagers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Main
        /// Gets a value indicating whether Main..
        /// </summary>
        public bool Main { get; set; }
    }
}
