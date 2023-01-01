using Auxquimia.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISearcheable<T, F> where F : ISearchFilter
    {
        /// <summary>
        /// Search on database by given fiter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IList<T>> SearchByFilter(FindRequestImpl<F> filter);
    }
}
