namespace Auxquimia.Service.Management.Factories
{
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using Auxquimia.Utils.MVC.Tools.Servs;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryService" />.
    /// </summary>
    public interface IFactoryService : IService<FactoryDto, Guid>, ISupportsSave<FactoryDto, Guid>, ISearchableService<FactoryListDto, FactorySearchFilter>
    {
        /// <summary>
        /// The findMainFactory.
        /// </summary>
        /// <returns>The <see cref="Task{FactoryDto}"/>.</returns>
        Task<FactoryDto> FindMainFactory();

        Task<FactoryListDto> FindMainFactoryList();
    }
}
