namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryManagerRepository" />.
    /// </summary>
    interface IFactoryManagerRepository : IRepositoryBase<FactoryManager>, ISupportsDelete<FactoryManager>, ISupportsSave<FactoryManager, Guid>, ISearcheable<FactoryManager, BaseSearchFilter>   
    {
        Task<FactoryManager> GetByManagerIdAndFactoryId(Guid managerId, Guid factoryId);
    }
}
