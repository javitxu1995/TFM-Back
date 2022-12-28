namespace Auxquimia.Utils.Kafka.Model
{
    using Auxquimia.Exceptions;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ProducerKafka" />.
    /// </summary>
    public class ProducerKafka : IDisposable
    {
        /// <summary>
        /// Gets or sets the Server_URL.
        /// </summary>
        private string Server_URL { get; set; }

        /// <summary>
        /// Gets or sets the Producer.
        /// </summary>
        private IProducer<Null, string> Producer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProducerKafka"/> class.
        /// </summary>
        /// <param name="server_Kafka">The server_Kafka<see cref="string"/>.</param>
        public ProducerKafka(string server_Kafka)
        {
            this.Server_URL = server_Kafka;
            Configure();
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        private void Configure()
        {
            ProducerConfig config = new ProducerConfig()
            {
                BootstrapServers = Server_URL,
#if DEBUG
                ClientId = "DEBUG_SATTELITE",
#else
                SecurityProtocol = SecurityProtocol.SaslPlaintext,
                ClientId = Dns.GetHostName(),
                SaslUsername = "auxquimia",
                SaslPassword = "bAcKLash+8",
                SaslMechanism = SaslMechanism.ScramSha512,
#endif
            };
            Producer = new ProducerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// The Produce.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<DeliveryResult<Null, string>> Produce(string topic, MessageKafka message)
        {
            if (Producer == null)
            {
                throw new CustomException(Constants.Kafka.Errors.PRODUCER_NOT_INITIALIZED);
            }
            string content = JsonConvert.SerializeObject(message);
            DeliveryResult<Null, string> result = await Producer.ProduceAsync(topic, new Message<Null, string> { Value = content });
            //Commit();
            return result;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (Producer != null)
            {
                Producer.Dispose();
            }
        }
        private void Commit()
        {
            if (Producer != null)
            {
                Producer.CommitTransaction();
            }
        }
    }
}
