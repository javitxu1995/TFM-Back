namespace Auxquimia.Dto.Authentication
{
    using System;

    /// <summary>
    /// Defines the <see cref="UserPassDto" />.
    /// </summary>
    [Serializable]
    public class UserPassDto
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }
    }
}
