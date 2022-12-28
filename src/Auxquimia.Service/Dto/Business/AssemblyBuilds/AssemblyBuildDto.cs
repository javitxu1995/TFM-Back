namespace Auxquimia.Dto.Business.AssemblyBuilds
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AssemblyBuildDto" />.
    /// </summary>
    [Serializable]
    public class AssemblyBuildDto
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
        [Required]
        public ReactorDto Blender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether OperatorApproval.
        /// </summary>
        public bool OperatorApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ManagerApproval.
        /// </summary>
        public bool ManagerApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether QcApproval.
        /// </summary>
        public bool QcApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NonConformite.
        /// </summary>
        public bool NonConformite { get; set; }

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
        /// Gets or sets the Operator.
        /// </summary>
        public UserDto Operator { get; set; }

        /// <summary>
        /// Gets or sets the Factory.
        /// </summary>
        public FactoryListDto Factory { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public FormulaDto Formula { get; set; }

        /// <summary>
        /// Gets or sets the NetsuiteFormula.
        /// </summary>
        public NetsuiteFormulaDto NetsuiteFormula { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NetsuiteWritted.
        /// </summary>
        public bool NetsuiteWritted { get; set; }

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
