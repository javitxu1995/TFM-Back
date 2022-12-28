namespace Auxquimia.Dto.Authentication
{
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Dto.Management.Factories;
    using Izertis.Misc.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The DTO for a Operator.
    /// </summary>
    [Serializable]
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CredentialsNonExpired.
        /// </summary>
        public bool CredentialsNonExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AccountNonExpired.
        /// </summary>
        public bool AccountNonExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AccountNonLocked.
        /// </summary>
        public bool AccountNonLocked { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        [Required]
        public CountryDto Country { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Roles.
        /// </summary>
        public IList<RoleDto> Roles { get; set; }

        /// <summary>
        /// Gets or sets the Factory.
        /// </summary>
        public FactoryListDto Factory { get; set; }

        /// <summary>
        /// Gets or sets the PasswordToken.
        /// </summary>
        public Guid PasswordToken { get; set; }

        /// <summary>
        /// Gets or sets the PasswordTokenExpiration.
        /// </summary>
        public long PasswordTokenExpiration { get; set; }

        /// <summary>
        /// Gets a value indicating whether Valid.
        /// </summary>
        public bool Valid
        {
            get
            {
                return Enabled && CredentialsNonExpired && AccountNonExpired && AccountNonLocked;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PasswordChanged.
        /// </summary>
        public bool PasswordChanged { get; set; }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            UserDto other = obj as UserDto;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Id, other.Id).IsEquals();
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
}
