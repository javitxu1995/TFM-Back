namespace Auxquimia.Config
{
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Service.Business.Kafka;
    using Auxquimia.Utils.Kafka;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="IAuxquimiaKafkaLauncher" />.
    /// </summary>
    public interface IAuxquimiaKafkaLauncher
    {
        /// <summary>
        /// The InitKafkaConsumers.
        /// </summary>
        void InitKafkaConsumers();
    }

    /// <summary>
    /// Defines the <see cref="AuxquimiaKafkaLauncher" />.
    /// </summary>
    public class AuxquimiaKafkaLauncher : IAuxquimiaKafkaLauncher
    {
        /// <summary>
        /// Defines the reactorRepository.
        /// </summary>
        private readonly IReactorRepository reactorRepository;

        private readonly IAuxquimiaKafkaService auxquimiaKafkaService;
        private readonly IContextConfigProvider contextConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuxquimiaKafkaLauncher"/> class.
        /// </summary>
        /// <param name="serviceRepository">The serviceRepository<see cref="IReactorRepository"/>.</param>
        public AuxquimiaKafkaLauncher(IReactorRepository serviceRepository, IAuxquimiaKafkaService auxquimiaKafkaService, IContextConfigProvider contextConfigProvider)
        {
            this.reactorRepository = serviceRepository;
            this.auxquimiaKafkaService = auxquimiaKafkaService;
            this.contextConfig = contextConfigProvider;

            //InitKafkaConsumers();
            InitKafkaService();
        }

        public void InitKafkaService()
        {
            
            auxquimiaKafkaService.InitConsumerThread();
        }

        /// <summary>
        /// The InitKafkaConsumers.
        /// </summary>
        public void InitKafkaConsumers()
        {
            Console.WriteLine("[KAFKA] Kafka process init");

            IList<Reactor> reactors;
            try
            {
                reactors = this.reactorRepository.GetAllSync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[KAFKA INIT EXCEPTION] Query exception: - {e.Message}");
                reactors = new List<Reactor>();
            }
            string endpoint = contextConfig.KafkaServer;
            foreach (Reactor reactor in reactors)
            {
                string topic = reactor.Id.ToString();
                KafkaAuxquimiaHelper.Get(endpoint).AddSubscription(topic);
            }

            KafkaAuxquimiaHelper.Get(endpoint).InitConsumerThread();
        }
    }
}
