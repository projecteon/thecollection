namespace TheCollection.Domain.Tests.Unit.Helpers {
    using System;
    using System.Collections.Generic;

    public class CountByComparer<T> : IEqualityComparer<CountBy<T>> where T: IComparable {
        public bool Equals(CountBy<T> x, CountBy<T> y) {
            return x.Value.Equals(y.Value) && x.Count == y.Count;
        }

        public int GetHashCode(CountBy<T> obj) {
            return obj.GetHashCode();
        }
    }
}
