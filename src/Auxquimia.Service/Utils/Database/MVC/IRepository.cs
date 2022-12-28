using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.Database.MVC
{
    public interface IRepository<T>
    {
        Task Save(T entity);
        Task Delete(T entity);
        Task<IList<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
    }
}
