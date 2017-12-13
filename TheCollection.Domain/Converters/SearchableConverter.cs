namespace TheCollection.Domain.Converters {
    using System;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SearchableConverter : JsonConverter {
        public override bool CanRead { get { return false; } }

        public override bool CanConvert(Type objectType) {
            return objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1).Any();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            JObject jo = new JObject();
            Type type = value.GetType();

            foreach (PropertyInfo prop in type.GetProperties()) {
                if (prop.CanRead) {
                    object propVal = prop.GetValue(value, null);
                    if (propVal != null) {
                        jo.Add(prop.Name.ToLower(), JToken.FromObject(propVal, serializer));
                    }
                    else {
                        jo.Add(prop.Name.ToLower(), null);
                    }
                }
            }

            var searchable = new Searchable(value);
            jo.Add(new JProperty(nameof(Searchable.Tags).ToLower(), new JArray(searchable.Tags)));
            jo.Add(new JProperty(nameof(Searchable.SearchString).ToLower(), searchable.SearchString));
            jo.WriteTo(writer);
        }
    }
}
