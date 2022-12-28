namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Factories;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryManagerRepository" />.
    /// </summary>
    interface IFactoryManagerRepository : ISupportsSave<FactoryManager, Guid>, IDao<FactoryManager, Guid>, ISearchableDao<FactoryManager, BaseSearchFilter>, ISupportsDelete<FactoryManager, Guid>
    {
        Task<FactoryManager> GetByManagerIdAndFactoryId(Guid managerId, Guid factoryId);
    }
}
