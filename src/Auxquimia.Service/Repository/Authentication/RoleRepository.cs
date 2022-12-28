namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Izertis.Misc.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RoleRepository" />.
    /// </summary>
    internal class RoleRepository : NHibernateRepository, IRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        /// <param name="sessionFactoryProvider">The sessionFactoryProvider<see cref="IFluentNhibernateLocalSessionFactoryProvider"/>.</param>
        public RoleRepository(IServiceProvider serviceProvider, IFluentNhibernateLocalSessionFactoryProvider sessionFactoryProvider) : base(serviceProvider, sessionFactoryProvider)
        {
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{Role}}"/>.</returns>
        public Task<IList<Role>> GetAllAsync()
        {
            return GetAllAsync<Role>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public Task<Role> GetAsync(Guid id)
        {
            return CurrentSession.QueryOver<Role>().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The getByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public Task<Role> getByName(string name)
        {
            return CurrentSession.QueryOver<Role>().Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{Role}}"/>.</returns>
        public Task<Page<Role>> PaginatedAsync(PageRequest pageRequest)
        {
            return PaginatedAsync(CurrentSession.QueryOver<Role>(), pageRequest);
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestImpl{RoleSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{Role}}"/>.</returns>
        public Task<Page<Role>> PaginatedAsync(FindRequestImpl<RoleSearchFilter> filter)
        {
            IQueryOver<Role, Role> qo = CurrentSession.QueryOver<Role>();

            if (filter.Filter != null)
            {
                RoleSearchFilter uFilter = filter.Filter;

                if (StringUtils.HasText(uFilter.Name))
                {
                    qo.And(Restrictions.On<Role>(x => x.Name).IsInsensitiveLike(uFilter.Name, MatchMode.Anywhere));
                }
            }

            return PaginatedAsync(qo, filter.PageRequest);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Role"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public async Task<Role> SaveAsync(Role entity)
        {
            await base.SaveAsync(entity).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{Role}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveAsync(IList<Role> entity)
        {
            await SaveAllAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="Role"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        public async Task<Role> UpdateAsync(Role entity)
        {
            return await CurrentSession.MergeAsync(entity).ConfigureAwait(false);
        }
    }
}
