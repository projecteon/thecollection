namespace TheCollection.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TheCollection.Lib.Extensions;

    public class Searchable : ISearchable
    {
        object SearchableObject { get; }
        readonly Lazy<IEnumerable<string>> tags;

        public Searchable(object searchableObject)
        {
            SearchableObject = searchableObject;
            tags = new Lazy<IEnumerable<string>>(() =>
                TheCollection.Business.Tags.Generate(GetSearchableValues(SearchableObject).Distinct().Where(value => value != null).Select(value => value.ToString()).Aggregate((current, next) => current + " " + next))
            );
        }

        public IEnumerable<string> Tags
        {
            get
            {
                return tags.Value;
            }
        }

        public string SearchString
        {
            get { return Tags.Aggregate((current, next) => current + " " + next); }
        }

        static IEnumerable<string> GetSearchableValues<Q>(Q objectValue)
        {
            return GetSearchablePrimitiveValues(objectValue).Concat(GetSearchableNonPrimitiveValues(objectValue));
        }

        static IEnumerable<PropertyInfo> GetSearchableProperties<Q>(Q objectValue)
        {
            return objectValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1);
        }

        static IEnumerable<string> GetSearchablePrimitiveValues<Q>(Q objectValue)
        {
            return GetSearchableProperties(objectValue).Where(prop => prop.PropertyType.IsSimpleType()).ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue) ?? null).Values.Where(value => value != null).Select(value => value.ToString());
        }

        static IEnumerable<string> GetSearchableNonPrimitiveValues<Q>(Q objectValue)
        {
            var props = GetSearchableProperties(objectValue);
            var validProps = props.Where(prop => !prop.PropertyType.IsSimpleType());
            var values = validProps.ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue, null) ?? null).Values.Where(value => value != null);
            return values.SelectMany(value => GetSearchableValues(value));
        }
    }
}