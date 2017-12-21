namespace TheCollection.Domain
{
    using System.Collections.Generic;
    using NodaTime;

    public class LocalDateComparer : IEqualityComparer<LocalDate> {
        public bool Equals(LocalDate localDateX, LocalDate localDateY) {
            return localDateX.Equals(localDateY);
        }

        public int GetHashCode(LocalDate localDate) {
            return localDate.GetHashCode();
        }
    }
}
