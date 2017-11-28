namespace TheCollection.Business {
    using System;
    using Constant;
    using Newtonsoft.Json;

    internal class ConstantConverter<T, G> : JsonConverter
        where G : Constant<T, G> where T : IComparable {
        public override bool CanConvert(Type objectType) {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Constant<,>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var id = serializer.Deserialize<T>(reader);
            if (id == null) {
                return null;
            }

            return Constant<T, G>.GetFor(id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if(value == null) {
                serializer.Serialize(writer, value);
            }

            var constant = (Constant<T, G>)value;
            serializer.Serialize(writer, constant.Key);

        }
    }
}
