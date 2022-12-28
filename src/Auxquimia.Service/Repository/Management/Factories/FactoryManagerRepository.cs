namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Factories;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FactoryManagerRepository" />.
    /// </summary>
    internal class FactoryManagerRepository : NHibernateRepository, IFactoryManagerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryManagerRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FactoryManagerRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryManager"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DeleteAsync(FactoryManager entity)
        {
            return CurrentSession.DeleteAsync(entity);
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public Task<int> DeleteAsync(Guid id)
        {
            IQuery query = CurrentSession.CreateQuery("delete M_FACTORY_MANAGER where Id = :id");
            query.SetGuid("id", id);

            return query.ExecuteUpdateAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FactoryManager}}"/>.</returns>
        public Task<IList<FactoryManager>> GetAllAsync()
        {
            return GetAllAsync<FactoryManager>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public Task<FactoryManager> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<FactoryManager>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByManagerIdAndFactoryId.
        /// </summary>
        /// <param name="managerId">The managerId<see cref="Guid"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public Task<FactoryManager> GetByManagerIdAndFactoryId(Guid managerId, Guid factoryId)
        {
            return CurrentSession.QueryOver<FactoryManager>().Where(x => x.Manager.Id == managerId && x.Factory.Id == factoryId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{FactoryManager}}"/>.</returns>
        public Task<Page<FactoryManager>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<FactoryManager>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FactoryManager}}"/>.</returns>
        public Task<Page<FactoryManager>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<FactoryManager, FactoryManager> qo = CurrentSession.QueryOver<FactoryManager>();


            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryManager"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public async Task<FactoryManager> SaveAsync(FactoryManager entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{FactoryManager}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<FactoryManager> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryManager"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public async Task<FactoryManager> UpdateAsync(FactoryManager entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
