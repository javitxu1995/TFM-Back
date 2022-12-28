namespace Auxquimia.Utils
{
    using Auxquimia.Config;
    using Izertis.Misc.Utils;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;

    /// <summary>
    /// Defines the <see cref="EmailUtils" />.
    /// </summary>
    public class EmailUtils
    {
        /// <summary>
        /// The SendEmail.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IContextConfigProvider"/>.</param>
        /// <param name="emailFrom">The emailFrom<see cref="string"/>.</param>
        /// <param name="destinationEmails">The destinationEmails<see cref="IList{string}"/>.</param>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="body">The body<see cref="string"/>.</param>
        /// <param name="attachmentFile">The attachmentFile<see cref="string"/>.</param>
        /// <param name="contentStream">The contentStream<see cref="Stream"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        public static void SendEmail(IContextConfigProvider configuration, string emailFrom, IList<string> destinationEmails, string subject, string body, string attachmentFile = null, Stream contentStream = null, string fileName = null)
        {
            SmtpClient client = new SmtpClient();
            client.Port = configuration.EmailPortOut;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = configuration.EmailServerOut;
            client.EnableSsl = configuration.EmailRequireSslOut;
            client.Credentials = new NetworkCredential(configuration.EmailAddress, configuration.EmailPass);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailFrom);

            // Destination emails
            //#if DEBUG
            //            destinationEmails.Clear();
            //            destinationEmails.Add("jlorences@izertis.com");
            //#endif

            foreach (string destEmail in destinationEmails)
            {
                mailMessage.To.Add(destEmail);
            }

            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            //check if exist attachment
            if (StringUtils.HasText(attachmentFile))
            {
                Attachment attachment = new Attachment(attachmentFile);
                mailMessage.Attachments.Add(attachment);
            }

            // Check if the attachment is a Stream
            if (contentStream != null)
            {
                Attachment attachment = new Attachment(contentStream, fileName, MediaTypeNames.Application.Pdf);
                mailMessage.Attachments.Add(attachment);
            }

            if (mailMessage.To.Count > 0)
            {
                client.Send(mailMessage);
            }
        }
    }
}
