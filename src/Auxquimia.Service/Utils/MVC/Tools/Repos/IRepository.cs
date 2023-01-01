using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools.Repos
{
    public interface IRepository<T, Guid>
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetAsync(Guid id);
    }
}
