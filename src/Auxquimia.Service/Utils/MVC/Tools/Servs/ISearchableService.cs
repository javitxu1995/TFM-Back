using Auxquimia.Filters;
using Auxquimia.Filters.FindRequests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools.Repos
{
    public interface ISearchableService<T, F> where F : ISearchFilter
    {
        /// <summary>
        /// Search on database by given fiter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IList<T>> SearchByFilter(FindRequestDto<F> filter);
    }
}
