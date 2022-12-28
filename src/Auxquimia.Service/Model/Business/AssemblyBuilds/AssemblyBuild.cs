namespace Auxquimia.Model.Business.AssemblyBuilds
{
    using Auxquimia.Enums;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Business.Formulas;
    using Auxquimia.Model.Management.Factories;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    //using Izertis.Misc.Utils;

    /// <summary>
    /// Defines the <see cref="AssemblyBuild" />.
    /// </summary>
    public class AssemblyBuild : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the AssemblyBuildNumber.
        /// </summary>
        public virtual string AssemblyBuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public virtual long Date { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyCode.
        /// </summary>
        public virtual string AssemblyCode { get; set; }

        /// <summary>
        /// Gets or sets the AssemblyName.
        /// </summary>
        public virtual string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the Blender.
        /// </summary>
        public virtual Reactor Blender { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public virtual Formula Formula { get; set; }

        /// <summary>
        /// Gets or sets the NetsuiteFormula.
        /// </summary>
        public virtual NetsuiteFormula NetsuiteFormula { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether OperatorApproval.
        /// </summary>
        public virtual bool OperatorApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ManagerApproval.
        /// </summary>
        public virtual bool ManagerApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether QcApproval.
        /// </summary>
        public virtual bool QcApproval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NonConformite.
        /// </summary>
        public virtual bool NonConformite { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public virtual ABStatus Status { get; set; } = ABStatus.PENDING;

        /// <summary>
        /// Gets or sets the ToProductionDate.
        /// </summary>
        public virtual long ToProductionDate { get; set; }

        /// <summary>
        /// Gets or sets the Deadline.
        /// </summary>
        public virtual long Deadline { get; set; }

        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public virtual User Operator { get; set; }

        /// <summary>
        /// Gets or sets the Factory.
        /// </summary>
        public virtual Factory Factory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NetsuiteWritted.
        /// </summary>
        public virtual bool NetsuiteWritted { get; set; }

        public virtual bool AbortSend { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether Aborted.
        /// </summary>
        public virtual bool Aborted { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as AssemblyBuild;
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
    /// Defines the <see cref="AssemblyBuildClassMap" />.
    /// </summary>
    internal class AssemblyBuildClassMap : ClassMap<AssemblyBuild>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildClassMap"/> class.
        /// </summary>
        public AssemblyBuildClassMap()
        {
            Table("ASSEMBLY_BUILD");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.AssemblyBuildNumber).Column("ASSEMBLY_BUILD_NUMBER");
            Map(x => x.Date).Column("DATE");
            Map(x => x.AssemblyCode).Column("ASSEMBLY_CODE");
            Map(x => x.AssemblyName).Column("ASSEMBLY_NAME");
            Map(x => x.OperatorApproval).Column("OPERATOR_APPROVAL");
            Map(x => x.ManagerApproval).Column("MANAGER_APPROVAL");
            Map(x => x.QcApproval).Column("QC_APPROVAL");
            Map(x => x.NonConformite).Column("NON_CONFORMITE");
            Map(x => x.Status).Column("STATUS");
            Map(x => x.ToProductionDate).Column("TO_PRODUCTION_DATE");
            Map(x => x.Deadline).Column("DEADLINE");
            Map(x => x.NetsuiteWritted).Column("NETSUITE_WRITTED").Not.Nullable();
            Map(x => x.AbortSend).Column("ABORT_SEND").Not.Nullable();
            Map(x => x.Aborted).Column("ABORTED").Not.Nullable();

            References(x => x.Operator).Column("ASSIGNED_OPERATOR").Index("IX_ASSEMBLY_BUILD_OPERATOR");
            References(x => x.Factory).Column("FACTORY_ID").Index("IX_ASSEMBLY_BUILD_FACTORY").Not.Nullable();
            References(x => x.Formula).Column("FORMULA_ID").Index("IX_ASSEMBLY_BUILD_FORMULA");
            References(x => x.NetsuiteFormula).Column("NETSUITE_FORMULA_ID").Index("IX_ASSEMBLY_BUILD_FORMULA");
            References(x => x.Blender).Column("BLENDER_ID").Index("IX_ASSEMBLY_BUILD_REACTOR").Not.Nullable();
        }
    }
}
