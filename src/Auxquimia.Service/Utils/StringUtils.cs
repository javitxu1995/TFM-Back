using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Utils
{
    public static class StringUtils
    {
        public static bool HasText(string text)
        {
            return !String.IsNullOrEmpty(text);
        }
    }
}
