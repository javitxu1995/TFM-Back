namespace Auxquimia.Service.Management.Factories
{
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters.Management.Factories;
    using Izertis.Interfaces.Abstractions;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IReactorService" />.
    /// </summary>
    public interface IReactorService : IService<ReactorDto, Guid>, ISupportsSave<ReactorDto, Guid>, ISearchableService<ReactorDto, ReactorSearchFilter>
    {
        /// <summary>
        /// The FindByCodeAsync.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        Task<ReactorDto> FindByCodeAsync(string code);

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        Task<ReactorDto> FindByNameAsync(string name);


    }
}
