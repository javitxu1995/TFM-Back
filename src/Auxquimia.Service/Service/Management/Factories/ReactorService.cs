namespace Auxquimia.Service.Management.Factories
{
    using Auxquimia.Config;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReactorService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class ReactorService : IReactorService
    {
        /// <summary>
        /// Defines the reactorRepository.
        /// </summary>
        private readonly IReactorRepository reactorRepository;

        /// <summary>
        /// Defines the contextConfigProvider.
        /// </summary>
        private readonly ContextConfigProvider contextConfigProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactorService"/> class.
        /// </summary>
        /// <param name="reactorRepository">The reactorRepository<see cref="IReactorRepository"/>.</param>
        /// <param name="contextConfigProvider">The contextConfigProvider<see cref="ContextConfigProvider"/>.</param>
        public ReactorService(IReactorRepository reactorRepository, ContextConfigProvider contextConfigProvider)
        {
            this.reactorRepository = reactorRepository;
            this.contextConfigProvider = contextConfigProvider;
        }

        /// <summary>
        /// The FindByCodeAsync.
        /// </summary>
        /// <param name="code">The code<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        public async Task<ReactorDto> FindByCodeAsync(string code)
        {
            var result = await reactorRepository.FindByCodeAsync(code).ConfigureAwait(false);
            return result.PerformMapping<Reactor, ReactorDto>();
        }

        /// <summary>
        /// The FindByNameAsync.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        public async Task<ReactorDto> FindByNameAsync(string name)
        {
            var result = await reactorRepository.FindByNameAsync(name).ConfigureAwait(false);
            return result.PerformMapping<Reactor, ReactorDto>();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{ReactorDto}}"/>.</returns>
        public async Task<IList<ReactorDto>> GetAllAsync()
        {
            var result = await reactorRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Reactor>, IList<ReactorDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        public async Task<ReactorDto> GetAsync(Guid id)
        {
            var result = await reactorRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Reactor, ReactorDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{ReactorDto}}"/>.</returns>
        public async Task<Page<ReactorDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await reactorRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Reactor>, Page<ReactorDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{ReactorSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{ReactorDto}}"/>.</returns>
        public async Task<Page<ReactorDto>> PaginatedAsync(FindRequestDto<ReactorSearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<ReactorSearchFilter>, FindRequestImpl<ReactorSearchFilter>>();
            var result = await reactorRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Reactor>, Page<ReactorDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="ReactorDto"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<ReactorDto> SaveAsync(ReactorDto entity)
        {
            Reactor reactor = entity.PerformMapping<ReactorDto, Reactor>();
            Reactor result = await reactorRepository.SaveAsync(reactor).ConfigureAwait(false);
            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{ReactorDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<ReactorDto> entity)
        {
            return reactorRepository.SaveAsync(entity.PerformMapping<IList<ReactorDto>, IList<Reactor>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="ReactorDto"/>.</param>
        /// <returns>The <see cref="Task{ReactorDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<ReactorDto> UpdateAsync(ReactorDto entity)
        {
            Reactor storedReactor = await reactorRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Reactor reactor = entity.PerformMapping(storedReactor);
            Reactor result = await reactorRepository.UpdateAsync(reactor).ConfigureAwait(false);
            return result.PerformMapping(entity);
        }
    }
}
