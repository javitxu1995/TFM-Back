namespace Auxquimia.Utils
{
    using Isopoh.Cryptography.Argon2;

    /// <summary>
    /// Defines the <see cref="CryptographyUtil" />
    /// </summary>
    public static class CryptographyUtil
    {
        /// <summary>
        /// take any string and encrypt it then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        public static string Encrypted(string data)
        {
            return Argon2.Hash(data);
        }

        /// <summary>
        /// Verify that the encrypted text is valid
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">The storedHashData<see cref="string"/></param>
        /// <returns>true or false depending on input validation</returns>
        public static bool ValidateHashData(string inputData, string storedHashData)
        {
            return Argon2.Verify(storedHashData, inputData);
        }
    }
}
