using System;
using System.Collections.Generic;
using System.Text;

namespace Fiuba.App7539.Helpers
{
    public static class StringHelper
    {
        public static string Join(IEnumerable<string> items)
        {
            return string.Join("; ", items);
        }
    }
}
