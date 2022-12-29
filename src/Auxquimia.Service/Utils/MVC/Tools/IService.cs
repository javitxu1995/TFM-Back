using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface IService<T, Guid>
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetAsync(Guid id);

        Task<T> SaveAsync(T entity);

        Task SaveAsync(IList<T> entity);

        Task<T> UpdateAsync(T entity);
    }
}
