namespace Auxquimia.Utils
{
    /// <summary>
    /// Defines the <see cref="HelperMethods" />.
    /// </summary>
    public class HelperMethods
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelperMethods"/> class.
        /// </summary>
        protected HelperMethods()
        {
        }

        /// <summary>
        /// The GetHash.
        /// </summary>
        /// <param name="input">The input<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetHash(string input)
        {
            return CryptographyUtil.Encrypted(input);
        }

        /// <summary>
        /// The BuildRFIDCode.
        /// </summary>
        /// <param name="stringA">The stringA<see cref="string"/>.</param>
        /// <param name="stringB">The stringB<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string BuildRFIDCode(string stringA, string stringB)
        {
            int maxInterSize = 16;
            string nA = BuildSemiRFIDCode(stringA, maxInterSize);
            string nB = BuildSemiRFIDCode(stringB, maxInterSize);
            string n = nA + nB;
            return n;
        }

        /// <summary>
        /// The BuildSemiRFIDCode.
        /// </summary>
        /// <param name="stringA">The stringA<see cref="string"/>.</param>
        /// <param name="maxSize">The maxSize<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string BuildSemiRFIDCode(string stringA, int maxSize)
        {
            if (stringA.Length < maxSize)
            {
                string nString = stringA;
                while (nString.Length < maxSize)
                {
                    nString = "0" + nString;
                }
                return nString;
            }
            else if (stringA.Length > maxSize)
            {
                string nString = stringA;
                while (nString.Length > maxSize)
                {
                    nString = nString.Substring(0, nString.Length - 2);
                }
                return nString;
            }
            else
            {
                return stringA;
            }
        }
    }
}
