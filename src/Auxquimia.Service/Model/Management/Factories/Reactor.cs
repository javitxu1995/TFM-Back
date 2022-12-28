namespace Auxquimia.Model.Management.Factories
{
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;

    /// <summary>
    /// Defines the <see cref="Reactor" />.
    /// </summary>
    public class Reactor : BaseNhibernateModel
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
        /// Gets or sets the Factory.
        /// </summary>
        public virtual Factory Factory { get; set; }

        /// <summary>
        /// Gets or sets the DbRead.
        /// </summary>
        public virtual int DbRead { get; set; }

        /// <summary>
        /// Gets or sets the DbWrite.
        /// </summary>
        public virtual int DbWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            Reactor other = obj as Reactor;
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
    /// Defines the <see cref="ReactorClassMap" />.
    /// </summary>
    internal class ReactorClassMap : ClassMap<Reactor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorClassMap"/> class.
        /// </summary>
        public ReactorClassMap()
        {
            Table("M_REACTOR");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Code).Column("CODE");
            Map(x => x.Name).Column("NAME");
            Map(x => x.DbRead).Column("DB_READ").Not.Nullable();
            Map(x => x.DbWrite).Column("DB_WRITE").Not.Nullable();
            Map(x => x.Enabled).Column("ENABLED");

            References(x => x.Factory).Column("FACTORY_ID");
        }
    }
}
