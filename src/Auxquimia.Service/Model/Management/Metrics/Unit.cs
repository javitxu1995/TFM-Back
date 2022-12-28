namespace Auxquimia.Model.Management.Metrics
{
    using FluentNHibernate.Mapping;
    using Izertis.Misc.Utils;

    /// <summary>
    /// Defines the <see cref="Unit" />.
    /// </summary>
    public class Unit : BaseNhibernateModel
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
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            Unit other = obj as Unit;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Code.ToUpper(), other.Code.ToUpper()).IsEquals();
            }
            return result;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(Id).ToHashCode();
        }
    }

    /// <summary>
    /// Defines the <see cref="UnitClassMap" />.
    /// </summary>
    internal class UnitClassMap : ClassMap<Unit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitClassMap"/> class.
        /// </summary>
        public UnitClassMap()
        {
            Table("UNIT");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Code).Column("CODE").Unique();
            Map(x => x.Name).Column("NAME").Unique();
        }
    }
}
