using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISupportsSave<T, Guid>
    {
        Task<T> Save(T entity);

        Task<T> Update(T entity);
    }
}
