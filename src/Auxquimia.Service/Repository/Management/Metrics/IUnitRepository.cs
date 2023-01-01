namespace Auxquimia.Repository.Management.Metrics
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Metrics;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Auxquimia.Utils.MVC.Tools;
    using Auxquimia.Utils.MVC.Tools.Repos;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUnitRepository" />.
    /// </summary>
    interface IUnitRepository : IRepositoryBase<Unit>, ISupportsSave<Unit, Guid>, ISearcheableRepository<Unit, BaseSearchFilter>
    {
        /// <summary>
        /// The FindByCode.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        Task<Unit> FindByCode(string code);

        /// <summary>
        /// The FindByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Unit}"/>.</returns>
        Task<Unit> FindByName(string name);
    }
}
