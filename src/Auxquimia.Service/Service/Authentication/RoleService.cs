namespace Auxquimia.Service.Authentication
{
    using Auxquimia.Dto.Authentication;
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Utils;
    using Izertis.NHibernate.Repositories;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RoleService" />.
    /// </summary>
    [Transaction(ReadOnly = true)]
    internal class RoleService : IRoleService
    {
        /// <summary>
        /// Defines the roleRepository.
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="roleRepository">The roleRepository<see cref="IRoleRepository"/>.</param>
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IList{RoleDto}}"/>.</returns>
        public async Task<IList<RoleDto>> GetAllAsync()
        {
            var result = await roleRepository.GetAllAsync().ConfigureAwait(false);
            return result.PerformMapping<IList<Role>, IList<RoleDto>>();
        }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        public async Task<RoleDto> GetAsync(Guid id)
        {
            var result = await roleRepository.GetAsync(id).ConfigureAwait(false);
            return result.PerformMapping<Role, RoleDto>();
        }

        /// <summary>
        /// The getAdminRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        public async Task<RoleDto> getAdminRole()
        {
            Role role = await roleRepository.getByName(Constants.Roles.ADMINISTRATOR);
            return role.PerformMapping<Role, RoleDto>();
        }

        /// <summary>
        /// The getManagerRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        public async Task<RoleDto> getManagerRole()
        {
            Role role = await roleRepository.getByName(Constants.Roles.MANAGER);
            return role.PerformMapping<Role, RoleDto>();
        }

        /// <summary>
        /// The getUserRole.
        /// </summary>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        public async Task<RoleDto> getUserRole()
        {
            Role role = await roleRepository.getByName(Constants.Roles.USER);
            return role.PerformMapping<Role, RoleDto>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="pageRequest">The pageRequest<see cref="PageRequest"/>.</param>
        /// <returns>The <see cref="Task{Page{RoleDto}}"/>.</returns>
        public async Task<Page<RoleDto>> PaginatedAsync(PageRequest pageRequest)
        {
            var result = await roleRepository.PaginatedAsync(pageRequest).ConfigureAwait(false);
            return result.PerformMapping<Page<Role>, Page<RoleDto>>();
        }

        /// <summary>
        /// The PaginatedAsync.
        /// </summary>
        /// <param name="filter">The filter<see cref="FindRequestDto{RoleSearchFilter}"/>.</param>
        /// <returns>The <see cref="Task{Page{RoleDto}}"/>.</returns>
        public async Task<Page<RoleDto>> PaginatedAsync(FindRequestDto<RoleSearchFilter> filter)
        {
            var findRequest = filter.PerformMapping<FindRequestDto<RoleSearchFilter>, FindRequestImpl<RoleSearchFilter>>();
            var result = await roleRepository.PaginatedAsync(findRequest).ConfigureAwait(false);

            return result.PerformMapping<Page<Role>, Page<RoleDto>>();
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="RoleDto"/>.</param>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<RoleDto> SaveAsync(RoleDto entity)
        {
            Role role = entity.PerformMapping<RoleDto, Role>();
            Role result = await roleRepository.SaveAsync(role).ConfigureAwait(false);



            return result.PerformMapping(entity);
        }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="IList{RoleDto}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Transaction(ReadOnly = false)]
        public Task SaveAsync(IList<RoleDto> entity)
        {
            return roleRepository.SaveAsync(entity.PerformMapping<IList<RoleDto>, IList<Role>>());
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="RoleDto"/>.</param>
        /// <returns>The <see cref="Task{RoleDto}"/>.</returns>
        [Transaction(ReadOnly = false)]
        public async Task<RoleDto> UpdateAsync(RoleDto entity)
        {
            Role storedRole = await roleRepository.GetAsync(entity.Id.PerformMapping<string, Guid>()).ConfigureAwait(false);
            Role role = entity.PerformMapping(storedRole);
            Role result = await roleRepository.UpdateAsync(role).ConfigureAwait(false);

            return result.PerformMapping(entity);
        }
    }
}
