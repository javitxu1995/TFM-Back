namespace Auxquimia.Model.Management.Factories
{
    using Auxquimia.Model.Management.Countries;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Factory" />.
    /// </summary>
    public class Factory : BaseNhibernateModel
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
        /// Gets or sets the Country.
        /// </summary>
        public virtual Country Country { get; set; }

        /// <summary>
        /// Gets or sets the Reactors.
        /// </summary>
        public virtual IList<Reactor> Reactors { get; set; } = new List<Reactor>();

        /// <summary>
        /// Gets or sets the FactoryManagers.
        /// </summary>
        public virtual IList<FactoryManager> FactoryManagers { get; set; } = new List<FactoryManager>();

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Main.
        /// </summary>
        public virtual bool Main { get; set; }

        /// <summary>
        /// Gets or sets the OpcServer.
        /// </summary>
        public virtual string OpcServer { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            Factory other = obj as Factory;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Code, other.Code).IsEquals;
            }
            return result;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(Code).GetHashCode();
        }

        /// <summary>
        /// The AddReactor.
        /// </summary>
        /// <param name="reactor">The reactor<see cref="FactoryReactor"/>.</param>
        public virtual void AddReactor(Reactor reactor)
        {
            reactor.Factory = this;
            this.Reactors.Add(reactor);
        }

        /// <summary>
        /// The RemoveReactor.
        /// </summary>
        /// <param name="reactor">The reactor<see cref="FactoryReactor"/>.</param>
        public virtual void RemoveReactor(Reactor reactor)
        {
            this.Reactors.Remove(reactor);
            reactor.Factory = null;
        }

        /// <summary>
        /// The AddManager.
        /// </summary>
        /// <param name="manager">The manager<see cref="FactoryManager"/>.</param>
        public virtual void AddManager(FactoryManager manager)
        {
            manager.Factory = this;
            this.FactoryManagers.Add(manager);
        }

        /// <summary>
        /// The RemoveManager.
        /// </summary>
        /// <param name="manager">The manager<see cref="FactoryManager"/>.</param>
        public virtual void RemoveManager(FactoryManager manager)
        {
            this.FactoryManagers.Remove(manager);
            manager.Factory = null;
        }
    }

    /// <summary>
    /// Defines the <see cref="FactoryClassMap" />.
    /// </summary>
    internal class FactoryClassMap : ClassMap<Factory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryClassMap"/> class.
        /// </summary>
        public FactoryClassMap()
        {
            Table("M_FACTORY");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Code).Column("CODE");
            Map(x => x.Name).Column("NAME");
            Map(x => x.Enabled).Column("ENABLED");
            Map(x => x.Main).Column("MAIN");
            Map(x => x.OpcServer).Column("OPC_SERVER").Not.Nullable();

            References(x => x.Country).Column("Country_ID").Index("IX_FACTORY_COUNTRY").Not.Nullable();

            HasMany(x => x.Reactors);
            HasMany(x => x.FactoryManagers);
        }
    }
}
