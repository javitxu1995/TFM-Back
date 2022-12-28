namespace Auxquimia.Dto.Authentication
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="RoleDtoOLD" />
    /// </summary>
    [Serializable]
    public class RoleDtoOLD
    {
        /// <summary>
        /// Gets or sets the Authority
        /// </summary>
        [Required]
        public string Authority { get; set; }
    }
}
