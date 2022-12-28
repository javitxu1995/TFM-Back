namespace Auxquimia.Utils.FileStorage
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="FtpHelper" />.
    /// </summary>
    public class FtpHelper : IDisposable
    {
        /// <summary>
        /// Defines the URL_ERROR_NOT_SET.
        /// </summary>
        private const string URL_ERROR_NOT_SET = "FTP server url must be set.";

        /// <summary>
        /// Defines the ZIP_FILE_TYPE.
        /// </summary>
        public readonly string ZIP_FILE_TYPE = "application/zip";

        /// <summary>
        /// Gets or sets the Server_url.
        /// </summary>
        private string Server_url { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Gets or sets the FilePath.
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Connected.
        /// </summary>
        private bool Connected { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        private FtpWebRequest request { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpHelper"/> class.
        /// </summary>
        /// <param name="server_url">The server_url<see cref="string"/>.</param>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="password">The password<see cref="string"/>.</param>
        /// <param name="filepath">The filepath<see cref="string"/>.</param>
        public FtpHelper(string server_url, string username, string password, string filepath)
        {
            Server_url = server_url;

            this.Username = username;
            this.FilePath = filepath;
            this.Password = password;
            request = null;
            this.Connect();
        }

        /// <summary>
        /// The Connect.
        /// </summary>
        private void Connect()
        {

            if (this.Server_url == null || this.Server_url.Equals(String.Empty))
            {
                throw new Exception(URL_ERROR_NOT_SET);
            }
            string path = "ftp://" + Server_url + "/" + FilePath;
            this.request = (FtpWebRequest)WebRequest.Create(path);
            request.Credentials = new NetworkCredential(this.Username, this.Password);

            this.Connected = true;
        }

        /// <summary>
        /// The getFileStream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream getFileStreamToRead()
        {
            if (request != null && Connected)
            {
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                return request.GetResponse().GetResponseStream();
            }
            return null;
        }

        /// <summary>
        /// The getFileStreamToWrite.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream getFileStreamToWrite()
        {
            if (request != null && Connected)
            {
                request.Method = WebRequestMethods.Ftp.UploadFile;
                return request.GetRequestStream();
            }
            return null;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            request.Abort();
            request = null;
        }
    }
}
