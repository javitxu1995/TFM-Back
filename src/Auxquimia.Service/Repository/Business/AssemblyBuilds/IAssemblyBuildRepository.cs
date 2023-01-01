namespace Auxquimia.Repository.Management.Business.AssemblyBuilds
{
    using Auxquimia.Filters.Business.AssemblyBuilds;
    using Auxquimia.Model.Business.AssemblyBuilds;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAssemblyBuildRepository" />.
    /// </summary>

    public interface IAssemblyBuildRepository : IRepositoryBase<AssemblyBuild>, ISupportsSave<AssemblyBuild, Guid>, ISearcheableRepository<AssemblyBuild, BaseAssemblyBuildSearchFilter>        
    {
        /// <summary>
        /// The Get TotallWoByStatus.
        /// </summary>
        /// <param name="filter">The filter<see cref="BaseAssemblyBuildSearchFilter"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int GetTotalWoByStatus(BaseAssemblyBuildSearchFilter filter);

        /// <summary>
        /// The GetByMultipleStatus.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseAssemblyBuildSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{AssemblyBuild}}"/>.</returns>
        Task<IList<AssemblyBuild>> GetByMultipleStatus(BaseAssemblyBuildSearchFilter filter);

        /// <summary>
        /// The GetToWaitingQueueAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        Task<AssemblyBuild> GetToWaitingQueueAsync(Guid id);

        /// <summary>
        /// The GetToProductionAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        Task<AssemblyBuild> GetToProductionAsync(Guid id);

        /// <summary>
        /// The GetSync.
        /// </summary>
        /// <param name="assemblyId">The assemblyId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="AssemblyBuild"/>.</returns>
        AssemblyBuild GetSync(Guid assemblyId);

        /// <summary>
        /// The GetAssembliesToNetsuite.
        /// </summary>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        Task<IList<AssemblyBuild>> GetAssembliesToNetsuite();

        /// <summary>
        /// The GetAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        Task<AssemblyBuild> GetAsyncWithSession(ISession session, Guid id);

        /// <summary>
        /// The UpdateAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        Task<AssemblyBuild> UpdateAsyncWithSession(ISession session, AssemblyBuild entity);

        /// <summary>
        /// The SaveAsyncWithSession.
        /// </summary>
        /// <param name="session">The session<see cref="ISession"/>.</param>
        /// <param name="entity">The entity<see cref="AssemblyBuild"/>.</param>
        /// <returns>The <see cref="Task{AssemblyBuild}"/>.</returns>
        Task<AssemblyBuild> SaveAsyncWithSession(ISession session, AssemblyBuild entity);

        /// <summary>
        /// The FindAssembliesWaitingToProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        Task<IList<AssemblyBuild>> FindAssembliesWaitingToProduction(Guid reactorId);

        /// <summary>
        /// The CheckAssembliesOnProduction.
        /// </summary>
        /// <param name="reactorId">The reactorId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{IList{AssemblyBuild}}"/>.</returns>
        Task<IList<AssemblyBuild>> CheckAssembliesOnProduction(Guid reactorId);
    }
}
