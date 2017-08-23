using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCollection.Web.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
