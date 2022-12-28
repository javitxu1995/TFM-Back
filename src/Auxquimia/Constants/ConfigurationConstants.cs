namespace Auxquimia
{
    /// <summary>
    /// Defines the <see cref="ConfigurationConstants" />
    /// </summary>
    public static class ConfigurationConstants
    {
        /// <summary>
        /// Defines the CORS_ENABLED
        /// </summary>
        public static readonly string CORS_ENABLED = "cors:enabled";

        /// <summary>
        /// Defines the CORS_ALLOWED_ORIGINS
        /// </summary>
        public static readonly string CORS_ALLOWED_ORIGINS = "cors:allowed_origins";

        /// <summary>
        /// Defines the CORS_ALLOWED_HEADERS
        /// </summary>
        public static readonly string CORS_ALLOWED_HEADERS = "cors:allowed_headers";

        /// <summary>
        /// Defines the CORS_ALLOWED_METHODS
        /// </summary>
        public static readonly string CORS_ALLOWED_METHODS = "cors:allowed_methods";

        /// <summary>
        /// Defines the CORS_EXPOSED_HEADERS
        /// </summary>
        public static readonly string CORS_EXPOSED_HEADERS = "cors:exposed_headers";

        /// <summary>
        /// Defines the CORS_ALLOW_CREDENTIALS
        /// </summary>
        public static readonly string CORS_ALLOW_CREDENTIALS = "cors:allow_credentials";

        /// <summary>
        /// Defines the IDENTITY_REDIS_ENABLED
        /// </summary>
        public static readonly string IDENTITY_REDIS_ENABLED = "identity_server:redis:enabled";

        /// <summary>
        /// Defines the IDENTITY_REDIS_CONNECTION_STRING
        /// </summary>
        public static readonly string IDENTITY_REDIS_CONNECTION_STRING = "identity_server:redis:connection_string";

        /// <summary>
        /// Defines the IDENTITY_USE_DEVELOPER_CREDENTIAL
        /// </summary>
        public static readonly string IDENTITY_USE_DEVELOPER_CREDENTIAL = "identity_server:use_developer_credential";

        /// <summary>
        /// Defines the IDENTITY_CREDENTIAL_PATH
        /// </summary>
        public static readonly string IDENTITY_CREDENTIAL_PATH = "identity_server:signing_credential_path";

        /// <summary>
        /// Defines the IDENTITY_CREDENTIAL_PASSWORD
        /// </summary>
        public static readonly string IDENTITY_CREDENTIAL_PASSWORD = "identity_server:signing_credential_password";

        /// <summary>
        /// Defines the IDENTITY_CREDENTIAL_REDIS_DB_NUM
        /// </summary>
        public static readonly string IDENTITY_CREDENTIAL_REDIS_DB_NUM = "identity_server:redis:db_num";

        /// <summary>
        /// Defines the IDENTITY_CREDENTIAL_REDIS_DB_PREFIX
        /// </summary>
        public static readonly string IDENTITY_CREDENTIAL_REDIS_DB_PREFIX = "identity_server:redis:db_prefix";

        /// <summary>
        /// Defines the IDENTITY_API_RESOURCES
        /// </summary>
        public static readonly string IDENTITY_API_RESOURCES = "identity_server:api_resources";

        /// <summary>
        /// Defines the IDENTITY_API_CLIENTS
        /// </summary>
        public static readonly string IDENTITY_API_CLIENTS = "identity_server:clients";
    }
}
