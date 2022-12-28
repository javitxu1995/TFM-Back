namespace Auxquimia.Dto.Business.AssemblyBuilds
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildListDto" />.
    /// </summary>
    public class AssemblyBuildListDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyBuildNumber.
        /// </summary>
        [Required]
        public string AssemblyBuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        [Required]
        public long Date { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyCode.
        /// </summary>
        public string AssemblyCode { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyName.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the Blender.
        /// </summary>
        public string Blender { get; set; }

        /// <summary>
        /// Gets or sets the BlenderName.
        /// </summary>
        public string BlenderName { get; set; }

        /// <summary>
        /// Gets or sets the FactoryName.
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// Gets or sets the FormulaName.
        /// </summary>
        public string FormulaName { get; set; }

        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public UserDto Operator { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public ABStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the ToProductionDate.
        /// </summary>
        public long ToProductionDate { get; set; }

        /// <summary>
        /// Gets or sets the Deadline.
        /// </summary>
        public long Deadline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AbortSend.
        /// </summary>
        public bool AbortSend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Aborted.
        /// </summary>
        public bool Aborted { get; set; }
    }
}
