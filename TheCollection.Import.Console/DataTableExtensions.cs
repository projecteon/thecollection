using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TheCollection.Import.Console {

    public static class DataTableExtensions {

        public static List<T> ToList<T>(this DataTable table) where T : class, new() {
            try {
                var properties = typeof(T).GetProperties().Where(property => property.CanWrite).ToList();
                var list = new List<T>(table.Rows.Count);
                foreach (var row in table.AsEnumerable().Skip(1)) {
                    var obj = new T();
                    foreach (var prop in properties) {
                        try {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);
                            prop.SetValue(obj, safeValue, null);
                        }
                        catch {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch {
                return new List<T>();
            }
        }
    }
}
