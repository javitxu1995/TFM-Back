namespace Auxquimia.Model.Management.Factories
{
    using Auxquimia.Model.Authentication;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;

    /// <summary>
    /// Defines the <see cref="FactoryManager" />.
    /// </summary>
    public class FactoryManager : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Factory.
        /// </summary>
        public virtual Factory Factory { get; set; }

        /// <summary>
        /// Gets or sets the Manager.
        /// </summary>
        public virtual User Manager { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            FactoryManager other = obj as FactoryManager;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder()
                    .Append(Factory, other.Factory)
                    .Append(Manager, other.Manager)
                    .IsEquals;
            }
            return result;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(Factory.GetHashCode()).Append(Manager.GetHashCode()).ToHashCode();
        }
    }

    /// <summary>
    /// Defines the <see cref="FactoryManagerClassMap" />.
    /// </summary>
    internal class FactoryManagerClassMap : ClassMap<FactoryManager>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryManagerClassMap"/> class.
        /// </summary>
        public FactoryManagerClassMap()
        {
            Table("M_FACTORY_MANAGER");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();

            References(x => x.Factory).Column("FACTORY_ID").Index("IX_FACTORYMANAGER").Not.Nullable();
            References(x => x.Manager).Column("MANAGER_ID").Index("IX_FACTORYMANAGER").Not.Nullable();
        }
    }
}
