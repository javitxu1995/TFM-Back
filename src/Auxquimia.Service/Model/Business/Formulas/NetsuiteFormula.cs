namespace Auxquimia.Model.Business.Formulas
{
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Model.Management.Metrics;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="NetsuiteFormula" />.
    /// </summary>
    public class NetsuiteFormula : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the Steps.
        /// </summary>
        public virtual IList<NetsuiteFormulaStep> Steps { get; set; } = new List<NetsuiteFormulaStep>();

        /// <summary>
        /// Gets or sets the BlenderFinalPercentaje.
        /// </summary>
        public virtual Int16 BlenderFinalPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the BlenderFinalTime.
        /// </summary>
        public virtual Int16 BlenderFinalTime { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyBuild.
        /// </summary>
        public virtual AssemblyBuild AssemblyBuild { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public virtual Unit Units { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDetailId.
        /// </summary>
        public virtual long InventoryDetailId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public virtual string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the BatchNumber.
        /// </summary>
        public virtual long BatchNumber { get; set; }

        /// <summary>
        /// Gets or sets the TotalWeight.
        /// </summary>
        public virtual float TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public virtual long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public virtual long EndDate { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            NetsuiteFormula other = obj as NetsuiteFormula;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Id, other.Id).IsEquals;
            }
            return result;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(Id).GetHashCode();
        }
    }

    /// <summary>
    /// Defines the <see cref="NetsuiteFormulaClassMap" />.
    /// </summary>
    internal class NetsuiteFormulaClassMap : ClassMap<NetsuiteFormula>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetsuiteFormulaClassMap"/> class.
        /// </summary>
        public NetsuiteFormulaClassMap()
        {
            Table("NETSUITE_FORMULA");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Name).Column("Name");
            Map(x => x.BlenderFinalPercentaje).Column("BLENDER_FINAL_PERCENTAJE");
            Map(x => x.BlenderFinalTime).Column("BLENDER_FINAL_TIME");
            Map(x => x.BatchNumber).Column("BATCH_NUMBER");
            Map(x => x.InventoryDetailId).Column("INVENTORY_DETAIL_ID");
            Map(x => x.InventoryLot).Column("INVENTORY_LOT");
            Map(x => x.TotalWeight).Column("TOTAL_WEIGHT");
            Map(x => x.StartDate).Column("START_DATE");
            Map(x => x.EndDate).Column("END_DATE");

            References(x => x.AssemblyBuild).Column("ASSEMBLY_BUILD_ID").Index("IX_FORMULA_ASSEMBLY_BUILD");
            HasMany(x => x.Steps);
            References(x => x.Units).Column("UNIT_ID");
        }
    }
}
