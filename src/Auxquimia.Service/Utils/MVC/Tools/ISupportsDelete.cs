using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISupportsDelete<T>
    {
        Task Delete(T entity);
    }
}
