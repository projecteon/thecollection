using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCollection.Lib.Extensions;

namespace TheCollection.Business
{
    public class Searchable : ISearchable
    {
        object SearchableObject { get; }

        public Searchable(object searchableObject)
        {
            SearchableObject = searchableObject;
        }

        public IEnumerable<string> Tags
        {
            get
            {
                return TheCollection.Business.Tags.Generate(GetSearchableValues(SearchableObject).Distinct().Where(value => value != null).Select(value => value.ToString()).Aggregate((current, next) => current + " " + next));
                //return Services.Tags.Generate(this.Flavour + " " + this.Brand?.Name + " " + this.Country?.Name + " " + this.SerialNumber + " " + this.Hallmark + " " + this.Serie + " " + this.Type?.Name);
            }
        }

        public string SearchString
        {
            get { return Tags.Aggregate((current, next) => current + " " + next); }
        }

        private IEnumerable<string> GetSearchableValues<Q>(Q objectValue)
        {
            return GetSearchablePrimitiveValues(objectValue).Concat(GetSearchableNonPrimitiveValues(objectValue));
        }

        private IEnumerable<PropertyInfo> GetSearchableProperties<Q>(Q objectValue)
        {
            return objectValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1);
        }

        private IEnumerable<string> GetSearchablePrimitiveValues<Q>(Q objectValue)
        {
            return GetSearchableProperties(objectValue).Where(prop => prop.PropertyType.IsSimpleType()).ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue) ?? null).Values.Where(value => value != null).Select(value => value.ToString());
        }

        private IEnumerable<string> GetSearchableNonPrimitiveValues<Q>(Q objectValue)
        {
            var props = GetSearchableProperties(objectValue);
            var validProps = props.Where(prop => !prop.PropertyType.IsSimpleType());
            var values = validProps.ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue, null) ?? null).Values.Where(value => value != null);
            return values.SelectMany(value => GetSearchableValues(value));
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchableAttribute : Attribute
    {
    }

    public class SearchableConverter : JsonConverter
    {
        public override bool CanRead { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1).Any();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //JToken t = JToken.FromObject(value);
            //if (t.Type != JTokenType.Object)
            //{
            //    t.WriteTo(writer);
            //}
            //else
            //{
            //    JObject o = (JObject)t;
            //    var searchable = new Searchable(value);

            //    o.Add(new JProperty(nameof(Searchable.Tags).ToLower(), new JArray(searchable.Tags)));
            //    o.Add(new JProperty(nameof(Searchable.SearchString).ToLower(), searchable.SearchString));

            //    o.WriteTo(writer);
            //}

            JObject jo = new JObject();
            Type type = value.GetType();

            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.CanRead)
                {
                    object propVal = prop.GetValue(value, null);
                    if (propVal != null)
                    {
                        jo.Add(prop.Name.ToLower(), JToken.FromObject(propVal, serializer));
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