namespace TheCollection.Domain {
    public class CountBy<T> {
        public CountBy(T value, int count) {
            Value = value;
            Count = count;
        }

        public T Value { get; }
        public int Count { get; }
    }
}
