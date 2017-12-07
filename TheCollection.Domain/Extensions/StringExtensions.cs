namespace TheCollection.Domain.Extensions {
    public static class StringExtensions {
        public static bool IsNullOrWhiteSpace(this string value) {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value) {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
