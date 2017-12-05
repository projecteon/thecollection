namespace TheCollection.Business.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions {
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            return !source.Any(predicate);
        }

        public static IEnumerable<U> Scan<T, U>(this IEnumerable<T> input, Func<U, T, U> next, U state) {
            yield return state;
            foreach (var item in input) {
                state = next(state, item);
                yield return state;
            }
        }
    }
}
