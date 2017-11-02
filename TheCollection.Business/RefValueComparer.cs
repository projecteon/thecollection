namespace TheCollection.Business
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RefValueComparer : IEqualityComparer<RefValue> {
        public bool Equals(RefValue refValueX, RefValue refValueY) {
            if (Object.ReferenceEquals(refValueX, refValueY)) return true;
            if (Object.ReferenceEquals(refValueX, null) || Object.ReferenceEquals(refValueY, null)) return false;
            if (refValueX.Id == refValueY.Id && refValueX.Name == refValueY.Name) return true;
            return false;
        }

        public int GetHashCode(RefValue refValue) {
            var hashId = refValue.Id.GetHashCode();
            var hashName = refValue.Name.GetHashCode();
            return hashId ^ hashName;
        }
    }
}
