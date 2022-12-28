namespace Auxquimia.Utils.Kafka
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Exceptions;
    using Auxquimia.Utils.Kafka.Enum;
    using Auxquimia.Utils.Kafka.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="KafkaAuxquimiaHelper" />.
    /// </summary>
    public class KafkaAuxquimiaHelper
    {
        /// <summary>
        /// Defines the instance.
        /// </summary>
        private static KafkaAuxquimiaHelper instance;

        /// <summary>
        /// Gets or sets the KafkaManager.
        /// </summary>
        private KafkaManager KafkaManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether KafkaLoop.
        /// </summary>
        private bool KafkaLoop { get; set; }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="endpoint">The endpoint<see cref="string"/>.</param>
        /// <returns>The <see cref="KafkaAuxquimiaHelper"/>.</returns>
        public static KafkaAuxquimiaHelper Get(string endpoint)
        {
            if (instance == null)
            {
                instance = new KafkaAuxquimiaHelper(endpoint);
            }
            return instance;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="KafkaAuxquimiaHelper"/> class from being created.
        /// </summary>
        /// <param name="endpoint">The endpoint<see cref="string"/>.</param>
        private KafkaAuxquimiaHelper(string endpoint)
        {
            KafkaManager = new KafkaManager(endpoint);
            KafkaLoop = true;
        }

        /// <summary>
        /// The Close.
        /// </summary>
        public void Close()
        {
            if (KafkaManager != null)
            {
                KafkaManager.Dispose();
            }
        }

        /// <summary>
        /// The WriteAssembly.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task WriteAssembly(AssemblyBuildDto assemblyBuild)
        {
            string topic = assemblyBuild.Blender.Id;
            string content = JsonConvert.SerializeObject(assemblyBuild);
            try
            {
                MessageKafka message = new MessageKafka(MessageType.PRODUCTION, content, KafkaManager.CONSUMER_NAME);
                //Escribir en Auxquimia
                await KafkaManager.Produce(topic + Constants.Kafka.Configuration.AUXQUIMIA_EXTENSION_TOPIC, message);
            }
            catch (Exception e)
            {
                throw new CustomException(Constants.Kafka.Errors.SERIALIZE_ERROR);
            }
        }

        /// <summary>
        /// The InitNewConsumer.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void InitNewConsumer(string topic)
        {
            //Leer de satelite
            KafkaManager.CreateConsumer(new string[] { (topic) },
                 (result) =>
                 {
                     if (result != null)
                     {
                         Console.WriteLine($"[KAFKA CONSUMER] Consumer from {result.Topic} received messeage.");
                         Console.WriteLine("---------");
                         Console.WriteLine($"[KAFKA CONSUMER] Message offset {result.Offset}.");
                         Console.WriteLine("---------");
                         if (result.Message != null && result.Message.Value != null)
                         {
                             string message = result.Message.Value;
                             try
                             {
                                 MessageKafka msn = JsonConvert.DeserializeObject<MessageKafka>(message);
                                 if (msn.Type == MessageType.INITIALIZATION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Initialization message");
                                 }
                                 else if (msn.Type == MessageType.PRODUCTION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Production message");
                                     AssemblyBuildDto assembly = JsonConvert.DeserializeObject<AssemblyBuildDto>(msn.Content);
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Assembly {assembly.AssemblyBuildNumber} recived! - Factory {assembly.Factory.Name}");
                                 }
                                 else if (msn.Type == MessageType.CONFIGURATION)
                                 {
                                     Console.WriteLine($"[KAFKA CONSUMER] ---> Configuration message");
                                 }

                             }
                             catch (Exception e)
                             {
                                 Console.WriteLine($"Kafka Message => Message is not an Kafka Message. \n Message: {message}");
                             }
                         }

                     }
                     else
                     {
                         Console.WriteLine("[KAFKA CONSUMER] No result after consume");
                     }
                 });
        }

        /// <summary>
        /// The AddSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        public void AddSubscription(string topic)
        {
            string topicModified = "";
            if (topic.Contains(Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC))
            {
                topicModified = topic;
            }
            else
            {
                topicModified = topic + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC;
            }
            if (!KafkaManager.IsConsumerCreated())
            {
                InitNewConsumer(topicModified);
            }
            else
            {
                KafkaManager.AddSubscription(topicModified);
            }
        }

        /// <summary>
        /// The GetSubscriptions.
        /// </summary>
        /// <returns>The <see cref="IList{string}"/>.</returns>
        public IList<string> GetSubscriptions()
        {
            IList<string> subs = new List<string>();
            if (KafkaManager != null)
            {
                subs = KafkaManager.GetSubscriptions();
            }
            return subs;
        }

        /// <summary>
        /// The ConsumeAll.
        /// </summary>
        public void ConsumeAll()
        {
            if (this.KafkaManager != null)
            {
                this.KafkaManager.ConsumeAll();
            }
        }

        /// <summary>
        /// The ClearAllTopics.
        /// </summary>
        public void ClearAllTopics()
        {

            if (this.KafkaManager != null)
            {
                this.KafkaManager.ClearConsumerTopics();
            }
        }

        /// <summary>
        /// The UpdateTopics.
        /// </summary>
        /// <param name="newTopics">The newTopics<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool UpdateTopics(IList<string> newTopics)
        {
            bool result = false;
            if (KafkaManager != null)
            {
                newTopics = newTopics.Select(x => x = x + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC).ToList();
                IList<string> actualTopics = GetSubscriptions();
                IList<string> topicsToAdd = newTopics.Except(actualTopics).ToList();
                IList<string> topicsToRemove = actualTopics.Except(newTopics).ToList();
                foreach (string sub in topicsToAdd)
                {
                    AddSubscription(sub);
                }
                foreach (string sub in topicsToRemove)
                {
                    RemoveSubscription(sub);
                }
                result = true;
            }

            return result;
        }

        /// <summary>
        /// The RemoveSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool RemoveSubscription(string topic)
        {
            bool result = false;
            if (KafkaManager != null)
            {
                IList<string> actualTopics = GetSubscriptions();
                topic = topic + Constants.Kafka.Configuration.SATELLITE_EXTENSION_TOPIC;
                if (actualTopics.Contains(topic))
                {
                    KafkaManager.RemoveSubscription(topic);
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// The Consume.
        /// </summary>
        public void Consume()
        {
            if (this.KafkaManager != null)
            {
                KafkaManager.Consume();
            }
        }

        /// <summary>
        /// The StopLoop.
        /// </summary>
        public void StopLoop()
        {
            KafkaLoop = false;
        }

        /// <summary>
        /// The RestartLoop.
        /// </summary>
        public void RestartLoop()
        {
            if (!KafkaLoop)
            {

                KafkaLoop = true;
                InitConsumerThread();
            }
        }

        /// <summary>
        /// The InitConsumerThread.
        /// </summary>
        public void InitConsumerThread()
        {
            Task.Run(
               () =>
               {
                   while (KafkaLoop)
                   {
                       Console.WriteLine("[KAFKA] Consuming...");
                       Console.WriteLine($"[KAFKA] Topic subscribed {GetSubscriptions().Count}");
                       Consume();
                       Thread.Sleep(10000);
                   }

               });
        }
    }
}
