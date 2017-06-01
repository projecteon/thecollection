using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCollection.Import.Console
{
    public static class IEnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            return Task.WhenAll(enumerable.Select(item => action(item)));
        }
    }
}
