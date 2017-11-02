namespace TheCollection.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class CountGroupBy<T, G, GC> where GC: IEqualityComparer<G>, new() {
        public CountGroupBy(IQueryable<T> queryable) {
            Queryable = queryable;
        }

        public IQueryable<T> Queryable { get; }

        public IEnumerable<CountBy> GroupAndCountBy(Expression<Func<T, G>> predicate) {
            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Queryable
                    .GroupBy(predicate, new GC())
                    .Select(x => new CountBy(x.Key, x.Count()));
        }

        public class CountBy {
            public CountBy(G value, int count) {
                Value = value;
                Count = count;
            }

            public G Value { get; }
            public int Count { get; }
        }
    }
}
