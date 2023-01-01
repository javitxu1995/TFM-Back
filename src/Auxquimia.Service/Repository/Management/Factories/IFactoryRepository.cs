namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryRepository" />.
    /// </summary>
    interface IFactoryRepository : IRepositoryBase<Factory>, ISupportsSave<Factory, Guid>, ISearcheableRepository<Factory, FactorySearchFilter>   
    {
        Task<Factory> FindMainFactory();
    }
}
