using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISupportsDelete<T>
    {
        Task<T> DeleteAsync(T entity);
        Task<int> DeleteAsync(Guid id);
    }
}
