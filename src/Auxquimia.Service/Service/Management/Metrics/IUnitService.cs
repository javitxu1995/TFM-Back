namespace Auxquimia.Service.Management.Metrics
{
    using Auxquimia.Dto.Management.Metrics;
    using Auxquimia.Filters;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUnitService" />.
    /// </summary>
    public interface IUnitService : IService<UnitDto, Guid>, ISupportsSave<UnitDto, Guid>, ISearchableService<UnitDto, BaseSearchFilter>
    {
        /// <summary>
        /// The FindByCode.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        Task<UnitDto> FindByCode(string code);

        /// <summary>
        /// The FindByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{UnitDto}"/>.</returns>
        Task<UnitDto> FindByName(string name);
    }
}
