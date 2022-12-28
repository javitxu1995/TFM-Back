namespace Auxquimia.Utils.Kafka
{
    using Auxquimia.Utils.Kafka.Enum;
    using Auxquimia.Utils.Kafka.Model;
    using Confluent.Kafka;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="KafkaManager" />.
    /// </summary>
    public class KafkaManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the Consumers.
        /// </summary>
        private IList<ConsumerKafka> Consumers { get; set; }

        /// <summary>
        /// Gets or sets the Producer.
        /// </summary>
        private ProducerKafka Producer { get; set; }

        /// <summary>
        /// Gets or sets the Consumer.
        /// </summary>
        private ConsumerKafka Consumer { get; set; }

        /// <summary>
        /// Gets or sets the KafkaEndpoint.
        /// </summary>
        private string KafkaEndpoint { get; set; }

        /// <summary>
        /// Defines the CONSUMER_NAME.
        /// </summary>
        public const string CONSUMER_NAME = "AUXQUIMIA";

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        private static KafkaManager instance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaManager"/> class.
        /// </summary>
        /// <param name="kafkaEndpoint">The kafkaEndpoint<see cref="string"/>.</param>
        public KafkaManager(string kafkaEndpoint)
        {
            this.KafkaEndpoint = kafkaEndpoint;
#if DEBUG
            Console.WriteLine($"[DEBUG KAFKA MANAGER] Kafka endpoint: {KafkaEndpoint}");
#endif
            Consumers = new List<ConsumerKafka>();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="kafkaEndpoint">The kafkaEndpoint<see cref="string"/>.</param>
        /// <returns>The <see cref="KafkaManager"/>.</returns>
        public static KafkaManager Get(string kafkaEndpoint)
        {
            if (instance == null)
            {
                instance = new KafkaManager(kafkaEndpoint);
            }
            return instance;
        }

        /// <summary>
        /// The CreateConsumer.
        /// </summary>
        /// <param name="topics">The topics<see cref="string[]"/>.</param>
        /// <param name="nextStep">The nextStep<see cref="Action{ConsumeResult{Null, string}}"/>.</param>
        public void CreateConsumer(string[] topics, Action<ConsumeResult<Null, string>> nextStep)
        {
            ConsumerKafka consumer = new ConsumerKafka(KafkaEndpoint);
            if (Consumers.Contains(consumer))
            {
                return;
            }
            consumer.Subscribe(topics, nextStep);
            this.Consumer = consumer;
            Consumers.Add(consumer);
        }

        /// <summary>
        /// The AddSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void AddSubscription(string topic)
        {
            if (this.Consumer != null)
            {
                Consumer.AddSubscription(topic);
            }
        }

        /// <summary>
        /// The RemoveSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void RemoveSubscription(string topic)
        {
            if (this.Consumer != null)
            {
                Consumer.RemoveSubscription(topic);
            }
        }

        /// <summary>
        /// The ClearConsumerTopics.
        /// </summary>
        public void ClearConsumerTopics()
        {
            if (Consumer != null)
            {
                Consumer.ClearTopics();
            }
        }

        /// <summary>
        /// The GetSubscriptions.
        /// </summary>
        /// <returns>The <see cref="IList{string}"/>.</returns>
        public IList<string> GetSubscriptions()
        {
            IList<string> subs = new List<string>();
            if (Consumer != null)
            {
                subs = Consumer.GetTopicsSubscribed();
            }
            return subs;
        }

        /// <summary>
        /// The IsConsumerCreated.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsConsumerCreated()
        {
            return Consumer != null;
        }

        /// <summary>
        /// The Consume.
        /// </summary>
        public void Consume()
        {
            if (this.Consumer != null)
            {
                try
                {
                    Consumer.Consume();
                }
                catch (ConsumeException eConsume)
                {
                    Console.WriteLine("Init topics for the first time.");
                    IList<string> topics = Consumer.GetTopicsSubscribed();
                    foreach (string t in topics)
                    {
                        Console.WriteLine($"[KAFKA] Init topic [{t}] for the first time.");
                        InitializeTopicForConsumer(t).ConfigureAwait(false);
                    }
                    Consumer.Consume();
                }
            }
        }

        /// <summary>
        /// The ConsumeAll.
        /// </summary>
        public void ConsumeAll()
        {
            foreach (ConsumerKafka consumer in Consumers)
            {
                try
                {
                    consumer.Consume();
                }
                catch (ConsumeException eConsume)
                {
                    Console.WriteLine("Init topics for the first time.");
                    IList<string> topics = consumer.GetTopicsSubscribed();
                    foreach (string t in topics)
                    {
                        Console.WriteLine($"[KAFKA] Init topic [{t}] for the first time.");
                        InitializeTopicForConsumer(t).ConfigureAwait(false);
                    }
                    consumer.Consume();
                    //await InitializeTopicForConsumer(
                }

            }
        }

        /// <summary>
        /// The InitializeTopicForConsumer.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task InitializeTopicForConsumer(string topic)
        {
            MessageKafka message = new MessageKafka(MessageType.INITIALIZATION, "Initializing topic", CONSUMER_NAME);
            await Produce(topic, message);
        }

        /// <summary>
        /// The Produce.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <param name="msn">The msn<see cref="string"/>.</param>
        /// <param name="actionResult">The actionResult<see cref="Action{DeliveryResult{Null, string}}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Produce(string topic, MessageKafka msn, Action<DeliveryResult<Null, string>> actionResult = null)
        {
            using (ProducerKafka producer = new ProducerKafka(KafkaEndpoint))
            {
                DeliveryResult<Null, string> result = await producer.Produce(topic, msn);
                if (actionResult != null)
                {
                    actionResult.Invoke(result);
                }
            }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            foreach (ConsumerKafka consumer in Consumers)
            {
                consumer.Dispose();
            }
            Consumer.Dispose();
            Producer.Dispose();
        }
    }
}
