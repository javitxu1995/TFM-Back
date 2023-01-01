using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Filters
{

    public class FindRequestImpl<T> where T : ISearchFilter
    {
        public T Filter { get; set; }
    }
}
