namespace Auxquimia.Model.Business.Formulas
{
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Management.Metrics;
    using FluentNHibernate.Mapping;
    using System;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStep" />.
    /// </summary>
    public class NetsuiteFormulaStep : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the QtyRequired.
        /// </summary>
        public virtual decimal QtyRequired { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public virtual Unit Units { get; set; }

        /// <summary>
        /// Gets or sets the BillMaterialsRevision.
        /// </summary>
        public virtual string BillMaterialsRevision { get; set; }

        /// <summary>
        /// Gets or sets the AdditionSequence.
        /// </summary>
        public virtual int AdditionSequence { get; set; }

        /// <summary>
        /// Gets or sets the RawQtyRequired.
        /// </summary>
        public virtual decimal RawQtyRequired { get; set; }

        /// <summary>
        /// Gets or sets the ItemCode.
        /// </summary>
        public virtual string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the ItemName.
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the RawMaterialUnits.
        /// </summary>
        public virtual Unit RawMaterialUnits { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDetailId.
        /// </summary>
        public virtual long InventoryDetailId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public virtual string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public virtual User Operator { get; set; }

        /// <summary>
        /// Gets or sets the StirringRate1.
        /// </summary>
        public virtual string StirringRate1 { get; set; }

        /// <summary>
        /// Gets or sets the StirringTime.
        /// </summary>
        public virtual Int16 StirringTime { get; set; }

        /// <summary>
        /// Gets or sets the BatchNumber.
        /// </summary>
        public virtual long BatchNumber { get; set; }

        /// <summary>
        /// Gets or sets the Temperature.
        /// </summary>
        public virtual string Temperature { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public virtual NetsuiteFormula Formula { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public virtual long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public virtual long EndDate { get; set; }

        /// <summary>
        /// Gets or sets the RealQtyProduced.
        /// </summary>
        public virtual decimal RealQtyProduced { get; set; }

        /// <summary>
        /// Gets or sets the RealQtyAdded.
        /// </summary>
        public virtual float RealQtyAdded { get; set; }

        /// <summary>
        /// Gets or sets the QuantityKg.
        /// </summary>
        public virtual decimal QuantityKg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Written.
        /// </summary>
        public virtual bool Written { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaStepClassMap" />.
    /// </summary>
    internal class NetsuiteFormulaStepClassMap : ClassMap<NetsuiteFormulaStep>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaStepClassMap"/> class.
        /// </summary>
        public NetsuiteFormulaStepClassMap()
        {
            Table("NETSUITE_FORMULA_STEP");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.QtyRequired).Column("QTY_REQUIRED");
            Map(x => x.BillMaterialsRevision).Column("BILL_MATERIALS_REVISION");
            Map(x => x.AdditionSequence).Column("ADDITION_SEQUENCE");
            Map(x => x.RawQtyRequired).Column("RAW_QTY_REQUIRED");
            Map(x => x.ItemCode).Column("ITEM_CODE");
            Map(x => x.ItemName).Column("ITEM_NAME");
            Map(x => x.InventoryDetailId).Column("INVENTORY_DETAIL_ID");
            Map(x => x.InventoryLot).Column("INVENTORY_LOT");
            Map(x => x.StirringRate1).Column("STIRRING_RATE_1");
            Map(x => x.StirringTime).Column("STIRRING_TIME");
            Map(x => x.BatchNumber).Column("BATCH_NUMBER");
            Map(x => x.Temperature).Column("TEMPERATURE");
            Map(x => x.StartDate).Column("START_DATE");
            Map(x => x.EndDate).Column("END_DATE");
            Map(x => x.RealQtyProduced).Column("REAL_QTY_PRODUCED");
            Map(x => x.RealQtyAdded).Column("REAL_QTY_ADDED");
            Map(x => x.QuantityKg).Column("QUANTITY_KG");
            Map(x => x.Written).Column("WRITTEN");

            References(x => x.Units).Column("UNIT_ID");
            References(x => x.RawMaterialUnits).Column("RAW_MATERIAL_UNIT_ID");
            References(x => x.Operator).Column("OPERATOR_ID");
            References(x => x.Formula).Column("NETSUITE_FORMULA_ID");
        }
    }
}
