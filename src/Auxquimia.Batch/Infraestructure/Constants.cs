using System;
namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The execution ok
        /// </summary>
        public const int EXECUTION_OK = 0;

        /// <summary>
        /// The internal error
        /// </summary>
        public const int INTERNAL_ERROR = -1;

        /// <summary>
        /// The connection error
        /// </summary>
        public const int CONNECTION_ERROR = -2;

        /// <summary>
        /// The database error
        /// </summary>
        public const int DATABASE_ERROR = -3;

        /// <summary>
        /// The authorization error
        /// </summary>
        public const int AUTHORIZATION_ERROR = -4;

        /// <summary>
        /// The invalid arguments error
        /// </summary>
        public const int INVALID_ARGS_ERROR = -5;
    }
}
