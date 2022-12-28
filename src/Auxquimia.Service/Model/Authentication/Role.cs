namespace Auxquimia.Model.Authentication
{
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System;

    /// <summary>
    /// Defines the <see cref="Role" />.
    /// </summary>
    public class Role : BaseNhibernateModel, IEquatable<Role>
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public virtual string Description { get; set; }

        public virtual bool AbSelectable { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            Role other = obj as Role;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Id, other.Id).IsEquals;
            }
            return result;
        }

        public bool Equals(Role other)
        {
            object otherobj = other;
            return Equals(otherobj);
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
    /// Defines the <see cref="RoleClassMap" />.
    /// </summary>
    internal class RoleClassMap : ClassMap<Role>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClassMap"/> class.
        /// </summary>
        public RoleClassMap()
        {
            Table("M_ROLE");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Name).Column("NAME");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.AbSelectable).Column("A_B_Selectable");
        }
    }
}
