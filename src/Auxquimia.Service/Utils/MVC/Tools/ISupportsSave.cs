using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISupportsSave<T, Guid>
    {
        /// <summary>
        /// Save async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> SaveAsync(T entity);
        /// <summary>
        /// Update async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);
    }
}
