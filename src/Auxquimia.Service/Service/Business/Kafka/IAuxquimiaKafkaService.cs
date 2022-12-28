namespace Auxquimia.Service.Business.Kafka
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Dto.Business.Formulas;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAuxquimiaKafkaService" />.
    /// </summary>
    public interface IAuxquimiaKafkaService
    {
        /// <summary>
        /// The WriteAssembly.
        /// </summary>
        /// <param name="assemblyBuild">The assemblyBuild<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task WriteAssembly(AssemblyBuildDto assemblyBuild);

        /// <summary>
        /// The InitNewConsumer.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        void InitNewConsumer(string topic);

        /// <summary>
        /// The AddSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        void AddSubscription(string topic);

        /// <summary>
        /// The GetSubscriptions.
        /// </summary>
        /// <returns>The <see cref="IList{string}"/>.</returns>
        IList<string> GetSubscriptions();

        /// <summary>
        /// The ConsumeAll.
        /// </summary>
        void ConsumeAll();

        /// <summary>
        /// The ClearAllTopics.
        /// </summary>
        void ClearAllTopics();

        /// <summary>
        /// The UpdateTopics.
        /// </summary>
        /// <param name="newTopics">The newTopics<see cref="IList{string}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool UpdateTopics(IList<string> newTopics);

        /// <summary>
        /// The RemoveSubscription.
        /// </summary>
        /// <param name="topic">The topic<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool RemoveSubscription(string topic);

        /// <summary>
        /// The Consume.
        /// </summary>
        void Consume();

        /// <summary>
        /// The StopLoop.
        /// </summary>
        void StopLoop();

        /// <summary>
        /// The RestartLoop.
        /// </summary>
        void RestartLoop();

        /// <summary>
        /// The InitConsumerThread.
        /// </summary>
        void InitConsumerThread();

        /// <summary>
        /// The UpdateKafkaConsumerTopics.
        /// </summary>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> UpdateKafkaConsumerTopics();

        /// <summary>
        /// The SendAssemblyBuildToKafka.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> SendAssemblyBuildToKafka(Guid assemblyId);

        /// <summary>
        /// The CheckAssembliesToSend.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> CheckAssembliesToSend(Guid reactorId);

        /// <summary>
        /// The SendAssemblyToWaitingQueue.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> SendAssemblyToProduction(Guid reactorId);

        /// <summary>
        /// The SendAssembliesToProduction.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        Task<IList<AssemblyBuildDto>> SendAssembliesToProduction();

        /// <summary>
        /// The SendAbortOrder.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> SendAbortOrder(Guid assemblyId);

        /// <summary>
        /// The SaveAndSendNetsuiteLotNewLot.
        /// </summary>
        /// <param name="lot">The lot<see cref="NetsuiteFormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> SaveAndSendNetsuiteLotNewLot(NetsuiteFormulaStepDto lot);

        /// <summary>
        /// The SaveAndSendFormulaLotNewLot.
        /// </summary>
        /// <param name="lot">The lot<see cref="FormulaStepDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> SaveAndSendFormulaLotNewLot(FormulaStepDto lot);
    }
}
