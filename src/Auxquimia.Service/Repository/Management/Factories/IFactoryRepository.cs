namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryRepository" />.
    /// </summary>
    interface IFactoryRepository : ISupportsSave<Factory, Guid>, IDao<Factory, Guid>, ISearchableDao<Factory, FactorySearchFilter>
    {
        Task<Factory> FindMainFactory();
    }
}
