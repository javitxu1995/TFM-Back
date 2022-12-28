namespace Auxquimia.Utils.Kafka.Model
{
    using Auxquimia.Exceptions;
    using Confluent.Kafka;
    using Izertis.Misc.Utils;
    using System;
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="ConsumerKafka" />.
    /// </summary>
    public class ConsumerKafka : IDisposable
    {
        /// <summary>
        /// Gets or sets the Server_URL.
        /// </summary>
        private string Server_URL { get; set; }

        /// <summary>
        /// Gets or sets the Consumer.
        /// </summary>
        private IConsumer<Null, string> Consumer { get; set; }

        /// <summary>
        /// Defines the Action.
        /// </summary>
        internal Action<ConsumeResult<Null, string>> Action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerKafka"/> class.
        /// </summary>
        /// <param name="server_Kafka">The server_Kafka<see cref="string"/>.</param>
        public ConsumerKafka(string server_Kafka)
        {
            this.Server_URL = server_Kafka;
            Configure();
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        private void Configure()
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = Server_URL,
                //Poner el mismo para todos o variar por la planta?
                StatisticsIntervalMs = 5000, //
                SessionTimeoutMs = 6000, //Para detectar si el broker esta vivo.
                AutoOffsetReset = AutoOffsetReset.Earliest, //Si no hay offset asignado
                //AllowAutoCreateTopics = true,
                EnableAutoCommit = true, //Deshabilitar auto commit para gestionarlo nosotros
                AutoCommitIntervalMs = 3000,
                GroupId = "AUXQUIMIA_CONSUMER",
#if DEBUG
                ClientId = "DEBUG_AUXQUIMIA",
#else
                ClientId = Dns.GetHostName(),
                EnablePartitionEof = true,
                SecurityProtocol = SecurityProtocol.SaslPlaintext,
                SaslUsername = "auxquimia",
                SaslPassword = "bAcKLash+8",
                SaslMechanism = SaslMechanism.ScramSha512
#endif
            };
            Consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// The Subscribe.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <param name="action">The action<see cref="Action{ConsumeResult{Null, string}}"/>.</param>
        public void Subscribe(string topic, Action<ConsumeResult<Null, string>> action)
        {
            if (Consumer == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            Consumer.Subscribe(topic);
            this.Action = action;
        }

        /// <summary>
        /// The Subscribe.
        /// </summary>
        /// <param name="topics">The topics<see cref="string[]"/>.</param>
        /// <param name="action">The action<see cref="Action{ConsumeResult{Null, string}}"/>.</param>
        public void Subscribe(string[] topics, Action<ConsumeResult<Null, string>> action)
        {
            if (Consumer == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            Consumer.Subscribe(topics);
            this.Action = action;
        }

        /// <summary>
        /// The AddSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void AddSubscription(string topic)
        {
            if (Consumer == null || Action == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            IList<string> previousTopics = GetTopicsSubscribed();
            if (!previousTopics.Contains(topic))
            {
                previousTopics.Add(topic);
            }
            Consumer.Subscribe(previousTopics);
        }

        /// <summary>
        /// The RemoveSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void RemoveSubscription(string topic)
        {
            if (Consumer == null || Action == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            IList<string> previousTopics = GetTopicsSubscribed();
            if (previousTopics.Contains(topic))
            {
                previousTopics.Remove(topic);
            }
            if (previousTopics.Count == 0)
            {
                //ResetConsumer();
                Consumer.Unsubscribe();
            }
            else
            {
                Consumer.Subscribe(previousTopics);
            }
        }

        /// <summary>
        /// The GetTopicsSubscribed.
        /// </summary>
        /// <returns>The <see cref="IList{string}"/>.</returns>
        public IList<string> GetTopicsSubscribed()
        {
            if (Consumer == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            IList<string> topics = Consumer.Subscription;
            return new List<string>(topics);
        }

        /// <summary>
        /// The ClearTopics.
        /// </summary>
        public void ClearTopics()
        {
            if (Consumer != null)
            {
                Consumer.Unsubscribe();
            }
        }

        /// <summary>
        /// The Consume.
        /// </summary>
        public void Consume()
        {
            if (Consumer == null)
            {
                throw new CustomException(Constants.Kafka.Errors.CONSUMER_NOT_INITIALIZED);
            }
            ConsumeResult<Null, string> result = Consumer.Consume(Constants.Kafka.Configuration.CONSUME_MAX_TIME_MILLISECONDS); //Max time consumings
            Action.Invoke(result);
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (Consumer != null)
            {
                Consumer.Dispose();
            }
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            ConsumerKafka other = obj as ConsumerKafka;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(GetTopicsSubscribed(), other.GetTopicsSubscribed()).IsEquals();
            }
            return result;
        }

        /// <summary>
        /// The Commit.
        /// </summary>
        public void Commit()
        {
            if (Consumer != null)
            {
                Consumer.Commit();
            }
        }
    }
}
