namespace TheCollection.Import.Console.Extensions {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class IEnumerableExtensions {

        public static Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action) {
            return Task.WhenAll(enumerable.Select(item => action(item)));
        }
    }
}
