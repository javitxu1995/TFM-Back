namespace Auxquimia.Dto.Business.Formulas
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Management.Metrics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaDto" />.
    /// </summary>
    [Serializable]
    public class NetsuiteFormulaDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Steps.
        /// </summary>
        public IList<NetsuiteFormulaStepDto> Steps { get; set; } = new List<NetsuiteFormulaStepDto>();

        /// <summary>
        /// Gets or sets the BlenderFinalPercentaje.
        /// </summary>
        public Int16 BlenderFinalPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the BlenderFinalTime.
        /// </summary>
        public Int16 BlenderFinalTime { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyBuild.
        /// </summary>
        public AssemblyBuildDto AssemblyBuild { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public UnitDto Units { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDetailId.
        /// </summary>
        public long InventoryDetailId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the BatchNumber.
        /// </summary>
        public long BatchNumber { get; set; }

        /// <summary>
        /// Gets or sets the TotalWeight.
        /// </summary>
        public float TotalWeight { get; set; }

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
