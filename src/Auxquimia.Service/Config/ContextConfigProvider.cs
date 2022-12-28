namespace Auxquimia.Config
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the <see cref="IContextConfigProvider" />.
    /// </summary>
    public interface IContextConfigProvider
    {
        /// <summary>
        /// Gets the OauthRequireSSL
        /// Gets a value indicating whether OauthRequireSSL...............
        /// </summary>
        bool OauthRequireSsl { get; }

        /// <summary>
        /// Gets the FtpServer.
        /// </summary>
        string FtpServer { get; }

        /// <summary>
        /// Gets the FtpFilePath_Read.
        /// </summary>
        string FtpFilePath_Read { get; }

        /// <summary>
        /// Gets the FtpFilePath_Write.
        /// </summary>
        string FtpFilePath_Write { get; }

        /// <summary>
        /// Gets the FtpUsername.
        /// </summary>
        string FtpUsername { get; }

        /// <summary>
        /// Gets the FtpPassword.
        /// </summary>
        string FtpPassword { get; }

        /// <summary>
        /// Gets the KafkaServer.
        /// </summary>
        string KafkaServer { get; }

        /// <summary>
        /// Gets the EmailServerOut.
        /// </summary>
        string EmailServerOut { get; }

        /// <summary>
        /// Gets the EmailRequireSslOut.
        /// </summary>
        bool EmailRequireSslOut { get; }

        /// <summary>
        /// Gets the EmailPortOut.
        /// </summary>
        int EmailPortOut { get; }

        /// <summary>
        /// Gets the EmailAddress.
        /// </summary>
        string EmailAddress { get; }

        /// <summary>
        /// Gets the EmailPass.
        /// </summary>
        string EmailPass { get; }

        /// <summary>
        /// Gets the EmailDelayedPendingStandardisations.
        /// </summary>
        string EmailDelayedPendingStandardisations { get; }

        /// <summary>
        /// Gets the EmailMaterialsPlanification.
        /// </summary>
        string EmailMaterialsPlanification { get; }

        /// <summary>
        /// Gets the EmailResetURL.
        /// </summary>
        string EmailResetURL { get; }
    }

    /// <summary>
    /// Defines the <see cref="ContextConfigProvider" />.
    /// </summary>
    public class ContextConfigProvider : IContextConfigProvider
    {
        /// <summary>
        /// Defines the OAUTH_REQUIRE_SSL.
        /// </summary>
        private readonly string OAUTH_REQUIRE_SSL = "oauth:require_ssl";

        /// <summary>
        /// Defines the FTP_SERVER.
        /// </summary>
        private readonly string FTP_SERVER = "ftp:server";

        /// <summary>
        /// Defines the FTP_FILEPATH_READ.
        /// </summary>
        private readonly string FTP_FILEPATH_READ = "ftp:filepath_read";

        /// <summary>
        /// Defines the FTP_FILEPATH_WRITE.
        /// </summary>
        private readonly string FTP_FILEPATH_WRITE = "ftp:filepath_write";

        /// <summary>
        /// Defines the FTP_USERNAME.
        /// </summary>
        private readonly string FTP_USERNAME = "ftp:username";

        /// <summary>
        /// Defines the FTP_PASSWORD.
        /// </summary>
        private readonly string FTP_PASSWORD = "ftp:password";

        /// <summary>
        /// Defines the KAFKA_SERVER.
        /// </summary>
        private readonly string KAFKA_SERVER = "kafka:server";

        /// <summary>
        /// Defines the EMAIL_SERVER_OUT.
        /// </summary>
        private readonly string EMAIL_SERVER_OUT = "email_config:email_server_out";

        /// <summary>
        /// Defines the EMAIL_REQUIRE_SSL_OUT.
        /// </summary>
        private readonly string EMAIL_REQUIRE_SSL_OUT = "email_config:email_require_ssl_out";

        /// <summary>
        /// Defines the EMAIL_PORT_OUT.
        /// </summary>
        private readonly string EMAIL_PORT_OUT = "email_config:email_port_out";

        /// <summary>
        /// Defines the EMAIL_ADDRESS.
        /// </summary>
        private readonly string EMAIL_ADDRESS = "email_config:email_address";

        /// <summary>
        /// Defines the EMAIL_PASS.
        /// </summary>
        private readonly string EMAIL_PASS = "email_config:email_pass";

        /// <summary>
        /// Defines the EMAIL_DELAYED_PENDING_STANDARDISATIONS.
        /// </summary>
        private readonly string EMAIL_DELAYED_PENDING_STANDARDISATIONS = "email_config:email_delayed_pending_standardisations";

        /// <summary>
        /// Defines the EMAIL_MATERIALS_PLANIFICATION.
        /// </summary>
        private readonly string EMAIL_MATERIALS_PLANIFICATION = "email_config:email_materials_planification";

        /// <summary>
        /// Defines the EMAIL_URL.
        /// </summary>
        private readonly string EMAIL_URL = "email_config:url";

        /// <summary>
        /// Defines the configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextConfigProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public ContextConfigProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets a value indicating whether OauthRequireSsl.
        /// </summary>
        public bool OauthRequireSsl => bool.Parse(configuration.GetValue<string>(OAUTH_REQUIRE_SSL));

        /// <summary>
        /// Gets the FtpServer.
        /// </summary>
        public string FtpServer => configuration.GetValue<string>(FTP_SERVER);

        /// <summary>
        /// Gets the FtpFilePath_Read.
        /// </summary>
        public string FtpFilePath_Read => configuration.GetValue<string>(FTP_FILEPATH_READ);

        /// <summary>
        /// Gets the FtpFilePath_Write.
        /// </summary>
        public string FtpFilePath_Write => configuration.GetValue<string>(FTP_FILEPATH_WRITE);

        /// <summary>
        /// Gets the FtpUsername.
        /// </summary>
        public string FtpUsername => configuration.GetValue<string>(FTP_USERNAME);

        /// <summary>
        /// Gets the FtpPassword.
        /// </summary>
        public string FtpPassword => configuration.GetValue<string>(FTP_PASSWORD);

        /// <summary>
        /// Gets the KafkaServer.
        /// </summary>
        public string KafkaServer => configuration.GetValue<string>(KAFKA_SERVER);

        /// <summary>
        /// Gets a value indicating whether EmailRequireSslOut.
        /// </summary>
        public bool EmailRequireSslOut => bool.Parse(configuration.GetValue<string>(EMAIL_REQUIRE_SSL_OUT));

        /// <summary>
        /// Gets the EmailPortOut.
        /// </summary>
        public int EmailPortOut => int.Parse(configuration.GetValue<string>(EMAIL_PORT_OUT));

        /// <summary>
        /// Gets the EmailAddress.
        /// </summary>
        public string EmailAddress => configuration.GetValue<string>(EMAIL_ADDRESS);

        /// <summary>
        /// Gets the EmailPass.
        /// </summary>
        public string EmailPass => configuration.GetValue<string>(EMAIL_PASS);

        /// <summary>
        /// Gets the EmailDelayedPendingStandardisations.
        /// </summary>
        public string EmailDelayedPendingStandardisations => configuration.GetValue<string>(EMAIL_DELAYED_PENDING_STANDARDISATIONS);

        /// <summary>
        /// Gets the EmailMaterialsPlanification.
        /// </summary>
        public string EmailMaterialsPlanification => configuration.GetValue<string>(EMAIL_MATERIALS_PLANIFICATION);

        /// <summary>
        /// Gets the EmailServerOut.
        /// </summary>
        public string EmailServerOut => configuration.GetValue<string>(EMAIL_SERVER_OUT);

        /// <summary>
        /// Gets the EmailResetURL.
        /// </summary>
        public string EmailResetURL => configuration.GetValue<string>(EMAIL_URL);
    }
}
