namespace TheCollection.Domain.Tests.Unit.Extensions {
    using Newtonsoft.Json;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;

    public static class LocalDateExtensions {
        public static string ToJson(this LocalDate localDate) {
            var serializer = new JsonSerializerSettings();
            serializer.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            return JsonConvert.SerializeObject(localDate, serializer);
        }
    }
}
