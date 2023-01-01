namespace Auxquimia.Repository.Management.Factories
{
    using Auxquimia.Filters;
    using Auxquimia.Model.Management.Factories;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FactoryManagerRepository" />.
    /// </summary>
    internal class FactoryManagerRepository : RepositoryBase<FactoryManager>, IFactoryManagerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryManagerRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public FactoryManagerRepository(IServiceProvider serviceProvider, NHibernateSessionProvider nHibernateSession) : base(serviceProvider, nHibernateSession)
        {
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryManager"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DeleteAsync(FactoryManager entity)
        {
            return _session.DeleteAsync(entity);
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public Task<int> DeleteAsync(Guid id)
        {
            IQuery query = _session.CreateQuery("delete M_FACTORY_MANAGER where Id = :id");
            query.SetGuid("id", id);

            return query.ExecuteUpdateAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{FactoryManager}}"/>.</returns>
        public override Task<IList<FactoryManager>> GetAllAsync()
        {
            return _session.QueryOver<FactoryManager>().ListAsync();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public override Task<FactoryManager> GetAsync(Guid id)
        {
            return _session.QueryOver<FactoryManager>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The GetByManagerIdAndFactoryId.
        /// </summary>
        /// <param name="managerId">The managerId<see cref="Guid"/>.</param>
        /// <param name="factoryId">The factoryId<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public Task<FactoryManager> GetByManagerIdAndFactoryId(Guid managerId, Guid factoryId)
        {
            return _session.QueryOver<FactoryManager>().Where(x => x.Manager.Id == managerId && x.Factory.Id == factoryId).SingleOrDefaultAsync();
        }


        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{BaseSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{FactoryManager}}"/>.</returns>
        public Task<IList<FactoryManager>> PaginatedAsync(FindRequestImpl<BaseSearchFilter> filter)
        {
            IQueryOver<FactoryManager, FactoryManager> qo = _session.QueryOver<FactoryManager>();


            return qo.ListAsync();
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
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="FactoryManager"/>.</param>
        /// <returns>The <see cref="Task{FactoryManager}"/>.</returns>
        public async override Task<FactoryManager> UpdateAsync(FactoryManager entity)
        {
            return await _session.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
