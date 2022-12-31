using Auxquimia.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxquimia.Utils.MVC.Tools
{
    public interface ISearcheable<T, F> where F : ISearchFilter
    {
        Task<IList<T>> SearchByFilter(F filter);
    }
}
