namespace Auxquimia.Model.Business.Formulas
{
    using Auxquimia.Model.Authentication;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System;

    /// <summary>
    /// Defines the <see cref="FormulaStep" />.
    /// </summary>
    public class FormulaStep : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public virtual Formula Formula { get; set; }

        /// <summary>
        /// Gets or sets the Step.
        /// </summary>
        public virtual int Step { get; set; }

        /// <summary>
        /// Gets or sets the SetPoint.
        /// </summary>
        public virtual float SetPoint { get; set; }

        /// <summary>
        /// Gets or sets the ItemCode.
        /// </summary>
        public virtual string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the ItemName.
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the InventoryLot.
        /// </summary>
        public virtual string InventoryLot { get; set; }

        /// <summary>
        /// Gets or sets the BlenderPercentaje.
        /// </summary>
        public virtual Int16 BlenderPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the RealWeight.
        /// </summary>
        public virtual float RealWeight { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public virtual long StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        public virtual long EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public virtual User Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Written.
        /// </summary>
        public virtual bool Written { get; set; }

        /// <summary>
        /// Gets or sets if the product is water or not
        /// </summary>
        public virtual bool IsWater { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            FormulaStep other = obj as FormulaStep;
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
    /// Defines the <see cref="FormulaStepClassMap" />.
    /// </summary>
    internal class FormulaStepClassMap : ClassMap<FormulaStep>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaStepClassMap"/> class.
        /// </summary>
        public FormulaStepClassMap()
        {
            Table("FORMULA_STEP");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Step).Column("STEP").Not.Nullable();
            Map(x => x.ItemCode).Column("ITEM_CODE");
            Map(x => x.ItemName).Column("ITEM_NAME");
            Map(x => x.SetPoint).Column("SETPOINT");
            Map(x => x.BlenderPercentaje).Column("BLENDER_PERCENTAJE");
            Map(x => x.StartDate).Column("START_DATE");
            Map(x => x.EndDate).Column("END_DATE");
            Map(x => x.RealWeight).Column("REAL_WEIGHT");
            Map(x => x.InventoryLot).Column("INVENTORY_LOT");
            Map(x => x.Written).Column("WRITTEN");
            Map(x => x.IsWater).Column("IS_WATER");


            References(x => x.Formula).Column("FORMULA_ID").Not.Nullable();
            References(x => x.Operator).Column("USER_ID");
        }
    }
}
