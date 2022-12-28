namespace Auxquimia.Model.Business.Formulas
{
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Formula" />.
    /// </summary>
    public class Formula : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the Steps.
        /// </summary>
        public virtual IList<FormulaStep> Steps { get; set; } = new List<FormulaStep>();

        /// <summary>
        /// Gets or sets the BlenderFinalPercentaje.
        /// </summary>
        public virtual Int16 BlenderFinalPercentaje { get; set; }

        /// <summary>
        /// Gets or sets the BlenderFinalTime.
        /// </summary>
        public virtual Int16 BlenderFinalTime { get; set; }

        /// <summary>
        /// Gets or sets the TotalWeight.
        /// </summary>
        public virtual float TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyBuild.
        /// </summary>
        public virtual AssemblyBuild AssemblyBuild { get; set; }

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
            Formula other = obj as Formula;
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
    /// Defines the <see cref="FormulaClassMap" />.
    /// </summary>
    internal class FormulaClassMap : ClassMap<Formula>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaClassMap"/> class.
        /// </summary>
        public FormulaClassMap()
        {
            Table("FORMULA");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Code).Column("CODE");
            Map(x => x.Name).Column("NAME");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.BlenderFinalPercentaje).Column("BLENDER_FINAL_PERCENTAJE");
            Map(x => x.BlenderFinalTime).Column("BLENDER_FINAL_TIME");
            Map(x => x.TotalWeight).Column("TOTAL_WEIGHT");
            Map(x => x.StartDate).Column("START_DATE");
            Map(x => x.EndDate).Column("END_DATE");

            References(x => x.AssemblyBuild).Column("ASSEMBLY_BUILD_ID").Index("IX_FORMULA_ASSEMBLY_BUILD");
            HasMany(x => x.Steps);
        }
    }
}
