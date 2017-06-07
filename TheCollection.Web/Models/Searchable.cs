using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCollection.Lib.Extensions;

namespace TheCollection.Web.Models
{
    public class Searchable<T> : ISearchable
    {
        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<string> Tags
        {            
            get
            {
                
                return Services.Tags.Generate(GetSearchableValues(this).Distinct().Where(value => value != null).Select(value => value.ToString()).Aggregate((current, next) => current + " " + next));
                //return Services.Tags.Generate(this.Flavour + " " + this.Brand?.Name + " " + this.Country?.Name + " " + this.SerialNumber + " " + this.Hallmark + " " + this.Serie + " " + this.Type?.Name);
            }
        }

        [JsonProperty(PropertyName = "searchstring")]
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
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1);
        }

        private IEnumerable<string> GetSearchablePrimitiveValues<Q>(Q objectValue)
        {
            return GetSearchableProperties(objectValue).Where(prop => prop.PropertyType.IsSimpleType()).ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue, null) ?? null).Values.Where(value => value != null).Select(value => value.ToString());
        }

        private IEnumerable<string> GetSearchableNonPrimitiveValues<Q>(Q objectValue)
        {
            var values = GetSearchableProperties(objectValue).Where(prop => !prop.PropertyType.IsSimpleType()).ToDictionary(prop => prop.Name, prop => prop.GetValue(this, null) ?? null).Values.Where(value => value != null);
            return values.SelectMany(value => GetSearchableValues(value));
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchableAttribute : Attribute
    {
    }
}
