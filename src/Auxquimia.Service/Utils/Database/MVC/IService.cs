using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.Database.MVC
{
    public interface IService<T>
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetAsync(Guid id);

        Task<T> SaveAsync(T entity);

        Task SaveAsync(IList<T> entity);

        Task<T> UpdateAsync(T entity);
    }
}
