namespace Auxquimia.Service.Business.AssemblyBuilds
{
    using Auxquimia.Dto.Business.AssemblyBuilds;
    using Auxquimia.Enums;
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Filters.FindRequests;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using Auxquimia.Utils.MVC.Tools.Servs;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAssemblyBuildService" />.
    /// </summary>
    public interface IAssemblyBuildService : IService<AssemblyBuildDto, Guid>, ISupportsSave<AssemblyBuildDto, Guid>, ISearchableService<AssemblyBuildListDto, BaseAssemblyBuildSearchFilter>
    {
        /// <summary>
        /// The GetCountWoByStatus.
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Dictionary{ABStatus, int}}"/>.</returns>
        Task<Dictionary<ABStatus, int>> GetCountWoByStatus(string userName, Guid factoryId);

        /// <summary>
        /// The GetByMultipleStatus.
        /// </summary>
        /// <param name="operatorName">The operatorName<see cref="string"/>.</param>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildListDto}}"/>.</returns>
        Task<IList<AssemblyBuildListDto>> GetByMultipleStatus(string operatorName, FindRequestDto<BaseAssemblyBuildSearchFilter> filter);

        /// <summary>
        /// The SearchByRole.
        /// </summary>
        /// <param name="operatorName">The operatorName<see cref="string"/>.</param>
        /// <param name="filter">The filter<see cref="FindRequestDto{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuildListDto}}"/>.</returns>
        Task<IList<AssemblyBuildListDto>> SearchByRole(string operatorName, FindRequestDto<BaseAssemblyBuildSearchFilter> filter);

        /// <summary>
        /// The LoadFromFtp.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        Task<IList<AssemblyBuildDto>> LoadFromFtp();

        /// <summary>
        /// The SendAssemblyBuildToOpc.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> SendAssemblyBuildToOpc(Guid assemblyId);

        /// <summary>
        /// The UpdateOperatorAsync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="operatorId">The operatorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> UpdateOperatorAsync(Guid assemblyId, Guid operatorId);

        /// <summary>
        /// The UpdateAssemblyFromSatellite.
        /// </summary>
        /// <param name="assemblyToUpdate">The assemblyToUpdate<see cref="AssemblyBuildDto"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> UpdateAssemblyFromSatellite(AssemblyBuildDto assemblyToUpdate);

        /// <summary>
        /// The GetToWaitingQueueAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> GetToWaitingQueueAsync(Guid assemblyBuildId);

        /// <summary>
        /// The GetToProductionAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> GetToProductionAsync(Guid assemblyBuildId);

        /// <summary>
        /// The ReloadWithNewLotAsync.
        /// </summary>
        /// <param name="assemblyBuildId">The assemblyBuildId<see cref="Guid"/>.</param>
        /// <param name="stepSequence">The stepSequence<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> ReloadWithNewLotAsync(Guid assemblyBuildId, int stepSequence = 0);

        /// <summary>
        /// The GetSync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="AssemblyBuildDto"/>.</returns>
        AssemblyBuildDto GetSync(Guid assemblyId);

        /// <summary>
        /// The SendToNetsuite.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuildListDto}}"/>.</returns>
        Task<IList<AssemblyBuildListDto>> SendToNetsuite();

        /// <summary>
        /// The SendAssemblyToWaitingQueue.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> SendAssemblyToWaitingQueue(Guid assemblyId);

        /// <summary>
        /// The FindAssembliesWaitingToProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuildDto}}"/>.</returns>
        Task<IList<AssemblyBuildDto>> FindAssembliesWaitingToProduction(Guid reactorId);

        /// <summary>
        /// The MarkStepsAsWritten.
        /// </summary>
        /// <param name="assemblyWritten">The assemblyWritten<see cref="AssemblyBuildDto"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> MarkStepsAsWritten(AssemblyBuildDto assemblyWritten, ISession session = null);

        /// <summary>
        /// The GetWithNewLotForStep.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequired">The stepRequired<see cref="int"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> GetWithNewLotForStep(Guid assemblyId, int stepRequired, ISession session = null);

        /// <summary>
        /// The NewLotAvailableForStep.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequired">The stepRequired<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> NewLotAvailableForStep(ISession session, Guid assemblyId, int stepRequired);

        /// <summary>
        /// The SendBackToProgress.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuildDto}"/>.</returns>
        Task<AssemblyBuildDto> SendBackToProgress(string username, Guid assemblyId);

        /// <summary>
        /// The CreateEmptyLot.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <param name="stepRequited">The stepRequited<see cref="int"/>.</param>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> CreateEmptyLot(Guid assemblyId, int stepRequited, ISession session = null);
    }
}
