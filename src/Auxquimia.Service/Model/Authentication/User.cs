namespace Auxquimia.Model.Authentication
{
    using Auxquimia.Model.Management.Countries;
    using Auxquimia.Model.Management.Factories;
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An authenticable user for the application.
    /// </summary>
    public class User : BaseNhibernateModel, IEquatable<User>
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the Surname.
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public virtual int? Code { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CredentialsNonExpired.
        /// </summary>
        public virtual bool CredentialsNonExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AccountNonExpired.
        /// </summary>
        public virtual bool AccountNonExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AccountNonLocked.
        /// </summary>
        public virtual bool AccountNonLocked { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the PasswordRecoveryHash.
        /// </summary>
        public virtual string PasswordRecoveryHash { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        public virtual Country Country { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// Gets or sets the Language.
        /// </summary>
        public virtual string Language { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Gets or sets the Roles.
        /// </summary>
        public virtual IList<UserRole> Roles { get; set; } = new List<UserRole>();

        /// <summary>
        /// Gets or sets the Factory.
        /// </summary>
        public virtual Factory Factory { get; set; }

        /// <summary>
        /// Gets or sets the PasswordToken.
        /// </summary>
        public virtual Guid PasswordToken { get; set; }

        /// <summary>
        /// Gets or sets the PasswordTokenExpiration.
        /// </summary>
        public virtual long PasswordTokenExpiration { get; set; }

        /// <summary>
        /// The AddRole.
        /// </summary>
        /// <param name="role">The role<see cref="UserRole"/>.</param>
        public virtual void AddRole(UserRole role)
        {
            role.User = this;
            this.Roles.Add(role);
        }

        /// <summary>
        /// The RemoveRole.
        /// </summary>
        /// <param name="role">The role<see cref="UserRole"/>.</param>
        public virtual void RemoveRole(UserRole role)
        {
            this.Roles.Remove(role);
            role.User = null;
        }

        /// <summary>
        /// Gets a value indicating whether Valid
        /// Determines the validity of the user. Abbreviates the possible invalid states in a single one.....
        /// </summary>
        public virtual bool Valid
        {
            get
            {
                return Enabled && CredentialsNonExpired && AccountNonExpired && AccountNonLocked;
            }
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            User other = obj as User;
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

        public bool Equals(User other)
        {
            object otherobj = other;
            return Equals(otherobj);
        }
    }

    /// <summary>
    /// Defines the <see cref="UserClassMap" />.
    /// </summary>
    internal class UserClassMap : ClassMap<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClassMap"/> class.
        /// </summary>
        public UserClassMap()
        {
            Table("APPLICATION_USER");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.Code).Column("CODE").Index("IX_USER_CODE");
            Map(x => x.Username).Column("USERNAME").Unique().Index("IX_USERNAME_PASSWORD");
            Map(x => x.Password).Column("PASSWORD").Index("IX_USERNAME_PASSWORD");
            Map(x => x.PasswordToken).Column("PASSWORD_TOKEN");
            Map(x => x.PasswordTokenExpiration).Column("PASSWORD_TOKEN_EXPIRATION");
            Map(x => x.Name).Column("NAME");
            Map(x => x.Surname).Column("SURNAME");
            Map(x => x.Email).Column("EMAIL");
            Map(x => x.Enabled).Column("ENABLED");
            Map(x => x.CredentialsNonExpired).Column("CREDENTIALS_NON_EXPIRED");
            Map(x => x.AccountNonExpired).Column("ACCOUNT_NON_EXPIRED");
            Map(x => x.AccountNonLocked).Column("ACCOUNT_NON_LOCKED");
            Map(x => x.PasswordRecoveryHash).Column("PASSWORD_RECOVERY_HASH");
            Map(x => x.City).Column("CITY");
            Map(x => x.Language).Column("LANGUAGE");
            Map(x => x.Address).Column("ADDRESS");
            HasMany(x => x.Roles);
            References(x => x.Factory).Column("FACTORY_ID").Index("IX_OPERATOR_FACOTRY").Not.Nullable();
            References(x => x.Country).Column("COUNTRY").Index("IX_OPERATOR_COUNTRY").Not.Nullable();
        }
    }
}
