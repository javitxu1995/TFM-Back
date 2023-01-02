using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Filters.FindRequests
{

    public class FindRequestImpl<T> : IFindRequest where T : ISearchFilter
    {
        public T Filter { get; set; }
    }
}
