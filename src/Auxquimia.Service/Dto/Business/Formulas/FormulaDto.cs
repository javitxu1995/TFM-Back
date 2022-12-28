namespace Auxquimia.Dto.Business.Formulas
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="FormulaDto" />.
    /// </summary>
    [Serializable]
    public class FormulaDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Steps.
        /// </summary>
        [Required]
        public IList<FormulaStepDto> Steps { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the BlenderFinalPercentaje.
        /// </summary>
        public Int16 BlenderFinalPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the BlenderFinalTime.
        /// </summary>
        public Int16 BlenderFinalTime { get; set; }

        /// <summary>
        /// Gets or sets the TotalWeight.
        /// </summary>
        public float TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyBuild.
        /// </summary>
        public AssemblyBuildDto AssemblyBuild { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public long EndDate { get; set; }
    }
}
