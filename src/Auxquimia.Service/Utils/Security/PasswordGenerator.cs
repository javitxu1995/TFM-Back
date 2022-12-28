namespace Auxquimia.Utils
{
    using System;

    /// <summary>
    /// Defines the <see cref="PassGenerator" />.
    /// </summary>
    public class PassGenerator
    {
        /// <summary>
        /// Defines the Random.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="PassGenerator"/> class.
        /// </summary>
        protected PassGenerator()
        {
        }

        /// <summary>
        /// The PasswordGenerator.
        /// </summary>
        /// <param name="passwordLength">The passwordLength<see cref="int"/>.</param>
        /// <param name="strongPassword">The strongPassword<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string PasswordGenerator(int passwordLength, bool strongPassword)
        {
            int seed = Random.Next(1, int.MaxValue);
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string specialCharacters = @"!#$%&'()*+,-./:;<=>?@[\]_";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                // If we are to use special characters
                chars[i] = strongPassword && i % Random.Next(3, passwordLength) == 0 ?
                    specialCharacters[rd.Next(0, specialCharacters.Length)] :
                    allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// The AlphanumericPasswordGenerator.
        /// </summary>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="strongPassword">The strongPassword<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string AlphanumericPasswordGenerator(int minLength, int maxLength)
        {
            int seed = Random.Next(1, int.MaxValue);
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

            if (minLength < 1)
            {
                minLength = 1;
            }

            if (minLength > maxLength)
            {
                minLength = maxLength - 1;
            }

            int passwordLength = Random.Next(minLength, maxLength);

            var chars = new char[passwordLength];
            var rd = new Random(seed);


            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
