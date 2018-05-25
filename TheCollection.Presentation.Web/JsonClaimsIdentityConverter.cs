namespace TheCollection.Presentation.Web {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonClaimsIdentityConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(ClaimsIdentity) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var jObject = JObject.Load(reader);
            var claims = jObject[nameof(ClaimsIdentity.Claims)].ToObject<IEnumerable<Claim>>(serializer);
            var authenticationType = (string)jObject[nameof(ClaimsIdentity.AuthenticationType)];
            var nameClaimType = (string)jObject[nameof(ClaimsIdentity.NameClaimType)];
            var roleClaimType = (string)jObject[nameof(ClaimsIdentity.RoleClaimType)];
            return new ClaimsIdentity(claims, authenticationType, nameClaimType, roleClaimType) { Label=(string)jObject[nameof(ClaimsIdentity.Label)] };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var claimsIdentity = (ClaimsIdentity)value;
            var jObject = new JObject {
                { nameof(ClaimsIdentity.Claims), new JArray(claimsIdentity.Claims.Select(x => JObject.FromObject(x, serializer))) },
                { nameof(ClaimsIdentity.AuthenticationType), claimsIdentity.AuthenticationType },
                { nameof(ClaimsIdentity.NameClaimType), claimsIdentity.NameClaimType },
                { nameof(ClaimsIdentity.RoleClaimType), claimsIdentity.RoleClaimType },
                { nameof(ClaimsIdentity.Label), claimsIdentity.Label }
            };

            jObject.WriteTo(writer);
        }
    }
}
