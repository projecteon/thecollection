namespace TheCollection.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TheCollection.Domain.Contracts;
    using TheCollection.Domain.Extensions;

    public class Searchable : ISearchable {
        private object SearchableObject { get; }
        private readonly Lazy<IEnumerable<string>> tags;

        public Searchable(object searchableObject) {
            SearchableObject = searchableObject;
            tags = new Lazy<IEnumerable<string>>(() =>
                TheCollection.Domain.Tags.Generate(
                    GetSearchableValues(SearchableObject)
                        .Distinct().Where(value => value != null)
                        .Select(value => value.ToString())
                        .Aggregate((current, next) => current + " " + next))
            );
        }

        public IEnumerable<string> Tags {
            get {
                return tags.Value;
            }
        }

        public string SearchString {
            get { return Tags.Aggregate((current, next) => current + " " + next); }
        }

        private static IEnumerable<string> GetSearchableValues<Q>(Q objectValue) {
            return GetSearchablePrimitiveValues(objectValue).Concat(GetSearchableNonPrimitiveValues(objectValue));
        }

        private static IEnumerable<PropertyInfo> GetSearchableProperties<Q>(Q objectValue) {
            return objectValue
                    .GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Where(p => p.GetCustomAttributes(typeof(SearchableAttribute), true).Count() == 1);
        }

        private static IEnumerable<string> GetSearchablePrimitiveValues<Q>(Q objectValue) {
            return GetSearchableProperties(objectValue)
                    .Where(prop => prop.PropertyType.IsSimpleType())
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue) ?? null)
                    .Values
                    .Where(value => value != null)
                    .Select(value => value.ToString());
        }

        private static IEnumerable<string> GetSearchableNonPrimitiveValues<Q>(Q objectValue) {
            var props = GetSearchableProperties(objectValue);
            var validProps = props.Where(prop => !prop.PropertyType.IsSimpleType());
            var values = validProps.ToDictionary(prop => prop.Name, prop => prop.GetValue(objectValue, null) ?? null).Values.Where(value => value != null);
            return values.SelectMany(value => GetSearchableValues(value));
        }
    }
}
