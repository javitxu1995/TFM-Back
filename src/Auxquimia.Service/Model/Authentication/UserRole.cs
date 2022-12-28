namespace Auxquimia.Model.Authentication
{
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;

    /// <summary>
    /// Defines the <see cref="UserRole" />.
    /// </summary>
    public class UserRole : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Operator.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            UserRole other = obj as UserRole;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder()
                    .Append<User>(User, other.User)
                    .Append<Role>(Role, other.Role)
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
            return new HashCodeBuilder().Append(Role.GetHashCode()).Append(User.GetHashCode()).GetHashCode();
        }
    }

    /// <summary>
    /// Defines the <see cref="UserRoleClassMap" />.
    /// </summary>
    internal class UserRoleClassMap : ClassMap<UserRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleClassMap"/> class.
        /// </summary>
        public UserRoleClassMap()
        {
            Table("M_USER_ROLE");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();

            References(x => x.User).Column("USER_ID").Index("IX_USERROLE").Not.Nullable();
            References(x => x.Role).Column("ROLE_ID").Index("IX_USERROLE").Not.Nullable();
        }
    }
}
