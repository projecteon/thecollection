namespace TheCollection.Import.Console.Extensions {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // https://blogs.msdn.microsoft.com/pfxteam/2012/03/05/implementing-a-simple-foreachasync-part-2/
    public static class IEnumerableExtensions {

        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action) {
            foreach (var item in enumerable) await action(item);
        }
    }
}
