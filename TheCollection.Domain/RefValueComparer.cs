namespace TheCollection.Domain {
    using System;
    using System.Collections.Generic;

    public class RefValueComparer : IEqualityComparer<RefValue> {
        public bool Equals(RefValue refValueX, RefValue refValueY) {
            if (Object.ReferenceEquals(refValueX, refValueY)) return true;
            if (Object.ReferenceEquals(refValueX, null) || Object.ReferenceEquals(refValueY, null)) return false;
            if (refValueX.Id == refValueY.Id) return true;
            return false;
        }

        public int GetHashCode(RefValue refValue) {
            var hashId = refValue.Id.GetHashCode();
            var hashName = refValue.Name.GetHashCode();
            return hashId ^ hashName;
        }
    }
}
