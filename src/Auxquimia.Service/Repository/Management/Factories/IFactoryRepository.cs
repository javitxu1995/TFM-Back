namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IFactoryRepository" />.
    /// </summary>
    interface IFactoryRepository : IRepositoryBase<Factory>, ISupportsSave<Factory, Guid>, ISearcheable<Factory, FactorySearchFilter>   
    {
        Task<Factory> FindMainFactory();
    }
}
