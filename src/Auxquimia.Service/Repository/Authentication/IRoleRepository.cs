namespace Auxquimia.Repository.Authentication
{
    using Auxquimia.Filters.Authentication;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Utils.MVC.InternalDatabase;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRoleRepository" />.
    /// </summary>
    interface IRoleRepository : IRepositoryBase<Role>
    //ISupportsSave<Role, Guid>, IDao<Role, Guid>, ISearchableDao<Role, RoleSearchFilter>
    {
        /// <summary>
        /// The getByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        Task<Role> getByName(string name);
    }
}
