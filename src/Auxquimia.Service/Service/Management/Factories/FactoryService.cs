namespace Auxquimia.Service.Management.Factories
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Filters.Management.Factories;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Repository.Management.Factories;
    using Auxquimia.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FactoryService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class FactoryService : IFactoryService
    {
        /// <summary>
        /// Defines the factoryRepository.
        /// </summary>
        private readonly IFactoryRepository factoryRepository;

        /// <summary>
        /// Defines the reactorRepository.
        /// </summary>
        private readonly IReactorRepository reactorRepository;

        /// <summary>
        /// Defines the factoryManagerRepository.
        /// </summary>
        private readonly IFactoryManagerRepository factoryManagerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryService"/> class.
        /// </summary>
        /// <param name="factoryRepository">The factoryRepository<see cref="IFactoryRepository"/>.</param>
        /// <param name="reactorRepository">The reactorRepository<see cref="IReactorRepository"/>.</param>
        /// <param name="factoryManagerRepository">The factoryManagerRepository<see cref="IFactoryManagerRepository"/>.</param>
        public FactoryService(IFactoryRepository factoryRepository, IReactorRepository reactorRepository, IFactoryManagerRepository factoryManagerRepository)
        {
            this.factoryRepository = factoryRepository;
            this.reactorRepository = reactorRepository;
            this.factoryManagerRepository = factoryManagerRepository;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FactoryDto}}"/>.</returns>
        public async Task<IList<FactoryDto>> GetAllAsync()
        {
            var result = await factoryRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Factory>, IList<FactoryDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FactoryDto}"/>.</returns>
        public async Task<FactoryDto> GetAsync(Guid id)
        {
            var result = await factoryRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Factory, FactoryDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{FactoryDto}}"/>.</returns>
        public async Task<Page<FactoryDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await factoryRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Factory>, Page<FactoryDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{FactorySearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FactoryDto}}"/>.</returns>
        public async Task<Page<FactoryListDto>> PaginatedAsync(FindRequestDto<FactorySearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<FactorySearchFilter>, FindRequestImpl<FactorySearchFilter>>();
            var result = await factoryRepository.PaginatedAsync(findRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Factory>, Page<FactoryListDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryDto"/>.</param>
        /// <returns>The <see cref="Task{FactoryDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FactoryDto> SaveAsync(FactoryDto entity)
        {
            Factory factory = entity.PerformMapping<FactoryDto, Factory>();
            Factory result = await factoryRepository.SaveAsync(factory).ConfigureAwait(false);

            // handle reactors
            result.Reactors = await this.HandleReactors(entity, result).ConfigureAwait(false);
            // handle managers
            result.FactoryManagers = await this.HandleManagers(entity, result).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{FactoryDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<FactoryDto> entity)
        {
            return factoryRepository.SaveAsync(entity.PerformMapping<IList<FactoryDto>, IList<Factory>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryDto"/>.</param>
        /// <returns>The <see cref="Task{FactoryDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<FactoryDto> UpdateAsync(FactoryDto entity)
        {
            Factory storedFactory = await factoryRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Factory factory = entity.PerformMapping(storedFactory);
            Factory result = await factoryRepository.UpdateAsync(factory).ConfigureAwait(false);

            // handle reactors
            result.Reactors = await this.HandleReactors(entity, result).ConfigureAwait(false);
            // handle managers
            result.FactoryManagers = await this.HandleManagers(entity, result).ConfigureAwait(false);


            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The HandleReactors.
        /// </summary>
        /// <param name="source">The source<see cref="FactoryDto"/>.</param>
        /// <param name="destination">The destination<see cref="Factory"/>.</param>
        /// <returns>The <see cref="Task{IList{FactoryReactor}}"/>.</returns>
        [Transaction(ReadOnly = false)]
        private async Task<IList<Reactor>> HandleReactors(FactoryDto source, Factory destination)
        {
            IList<Reactor> reactors = source.Reactors.PerformMapping<IList<ReactorDto>, IList<Reactor>>();
            reactors = reactors ?? new List<Reactor>();
            IList<Reactor> actualReactors = destination.Reactors;

            // DeleteAsync no longer Reactors
            IList<Reactor> removedReactors = actualReactors;
            if (removedReactors.Any())
            {
                removedReactors = removedReactors.Except(reactors).ToList();
                foreach (Reactor reactor in removedReactors)
                {
                    reactor.Enabled = false;
                    // upload to disabled
                    await reactorRepository.UpdateAsync(reactor).ConfigureAwait(false);
                }
            }

            IList<Reactor> newReactors = reactors.Where(x => x.Id == default(Guid)).ToList();
            reactors = reactors.Except(newReactors).ToList();

            //Update existing reactors
            foreach (Reactor existingReactor in reactors)
            {
                existingReactor.Factory = destination;
                Reactor updatedReactor = await this.reactorRepository.UpdateAsync(existingReactor).ConfigureAwait(false);
            }

            // Add new Reactors
            if (newReactors.Any())
            {
                foreach (Reactor reactor in newReactors)
                {
                    reactor.Factory = destination;
                    // Save on BBDD
                    Reactor reactorSaved = await reactorRepository.SaveAsync(reactor).ConfigureAwait(false);
                    destination.Reactors.Add(reactorSaved);
                }
            }

            return destination.Reactors;
        }

        /// <summary>
        /// The HandleManagers.
        /// </summary>
        /// <param name="source">The source<see cref="FactoryDto"/>.</param>
        /// <param name="destination">The destination<see cref="Factory"/>.</param>
        /// <returns>The <see cref="Task{IList{FactoryManager}}"/>.</returns>
        private async Task<IList<FactoryManager>> HandleManagers(FactoryDto source, Factory destination)
        {
            IList<User> managers = source.FactoryManagers.PerformMapping<IList<UserDto>, IList<User>>();
            IList<User> actualManagers = new List<User>();
            foreach (FactoryManager factoryManager in destination.FactoryManagers)
            {
                actualManagers.Add(factoryManager.Manager);
            }

            // DeleteAsync no longer managers
            IList<User> removedManagers = actualManagers;
            if (removedManagers.Any())
            {
                removedManagers = removedManagers.Except(managers).ToList();
                foreach (User manager in removedManagers)
                {
                    FactoryManager factoryManager = new FactoryManager
                    {
                        Factory = destination,
                        Manager = manager
                    };

                    //Borrar de la BBDD
                    FactoryManager deleteFactoryManager = await factoryManagerRepository.GetByManagerIdAndFactoryId(manager.Id, destination.Id).ConfigureAwait(false);
                    await factoryManagerRepository.DeleteAsync(deleteFactoryManager).ConfigureAwait(false);
                }
            }

            if (managers.Any())
            {
                IList<User> addedUsers = managers.Except(actualManagers).ToList();
                foreach (User newManager in addedUsers)
                {
                    FactoryManager factoryManager = new FactoryManager()
                    {
                        Manager = newManager
                    };
                    //Añadir los managers
                    destination.AddManager(factoryManager);
                    //Guardar en BBDD
                    await factoryManagerRepository.SaveAsync(factoryManager).ConfigureAwait(false);
                }
            }
            return destination.FactoryManagers;
        }

        /// <summary>
        /// The FindMainFactory.
        /// </summary>
        /// <returns>The <see cref="Task{FactoryDto}"/>.</returns>
        public async Task<FactoryDto> FindMainFactory()
        {
            Factory mainFactory = await this.factoryRepository.FindMainFactory();
            return mainFactory.PerformMapping<Factory, FactoryDto>();
        }

        /// <summary>
        /// The FindMainFactoryList.
        /// </summary>
        /// <returns>The <see cref="Task{FactoryListDto}"/>.</returns>
        public async Task<FactoryListDto> FindMainFactoryList()
        {
            Factory mainFactory = await this.factoryRepository.FindMainFactory();
            return mainFactory.PerformMapping<Factory, FactoryListDto>();
        }
    }
}
