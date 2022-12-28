namespace Auxquimia.Dto.Business.Formulas
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Management.Metrics;
    using System;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStepDto" />.
    /// </summary>
    public class NetsuiteFormulaStepDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the QtyRequired.
        /// </summary>
        public decimal QtyRequired { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public UnitDto Units { get; set; }

        /// <summary>
        /// Gets or sets the BillMaterialsRevision.
        /// </summary>
        public string BillMaterialsRevision { get; set; }

        /// <summary>
        /// Gets or sets the AdditionSequence.
        /// </summary>
        public int AdditionSequence { get; set; }

        /// <summary>
        /// Gets or sets the RawQtyRequired.
        /// </summary>
        public decimal RawQtyRequired { get; set; }

        /// <summary>
        /// Gets or sets the ItemCode.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the ItemName.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the RawMaterialUnits.
        /// </summary>
        public UnitDto RawMaterialUnits { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDetailId.
        /// </summary>
        public long InventoryDetailId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public UserDto Operator { get; set; }

        /// <summary>
        /// Gets or sets the StirringRate1.
        /// </summary>
        public string StirringRate1 { get; set; }

        /// <summary>
        /// Gets or sets the StirringTime.
        /// </summary>
        public Int16 StirringTime { get; set; }

        /// <summary>
        /// Gets or sets the BatchNumber.
        /// </summary>
        public long BatchNumber { get; set; }

        /// <summary>
        /// Gets or sets the Temperature.
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public NetsuiteFormulaDto Formula { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public long EndDate { get; set; }

        /// <summary>
        /// Gets or sets the RealQtyProduced.
        /// </summary>
        public decimal RealQtyProduced { get; set; }

        /// <summary>
        /// Gets or sets the RealQtyAdded.
        /// </summary>
        public float RealQtyAdded { get; set; }

        /// <summary>
        /// Gets or sets the QuantityKg.
        /// </summary>
        public decimal QuantityKg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Written.
        /// </summary>
        public bool Written { get; set; }
    }
}
