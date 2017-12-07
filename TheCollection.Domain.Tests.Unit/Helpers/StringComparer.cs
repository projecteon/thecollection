namespace TheCollection.Domain.Tests.Unit.Helpers
{
    using System.Collections.Generic;

    public class StringComparer : IEqualityComparer<string> {
        public bool Equals(string stringX, string stringY) {
            return stringX.Equals(stringY);
        }

        public int GetHashCode(string stringValue) {
            return stringValue.GetHashCode();
        }
    }
}
