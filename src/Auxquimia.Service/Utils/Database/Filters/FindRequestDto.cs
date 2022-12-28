using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Utils.Database.Filters
{
    public class FindRequestDto<T>
    {
        public T filter { get; set; }

        public FindRequestDto(T filter)
        {
            this.filter = filter;
        }
    }
}
