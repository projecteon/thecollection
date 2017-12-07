namespace TheCollection.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class CountGroupBy<T, G, GC> where GC: IEqualityComparer<G>, new() {
        public CountGroupBy(IQueryable<T> queryable) {
            Queryable = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }

        public IQueryable<T> Queryable { get; }

        public IEnumerable<CountBy<G>> GroupAndCountBy(Expression<Func<T, G>> predicate) {
            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Queryable
                    .GroupBy(predicate, new GC())
                    .Select(x => new CountBy<G>(x.Key, x.Count()));
        }
    }
}
