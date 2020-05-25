using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ManejadorBD
{
    public static class ConvertBD
    {
        #region equal convert
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }
        public static T ToObject<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            T result = new T();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result = item;
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                        property.SetValue(item, row[property.Name], null);
                }
            }
            return item;
        }
        #endregion

        #region distinc convert
        public static List<T> ToList<T>(this DataTable table, Dictionary<string, string> _dictionary) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties, _dictionary);
                result.Add(item);
            }

            return result;
        }
        public static T ToObject<T>(this DataTable table, Dictionary<string, string> _dictionary) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            T result = new T();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties, _dictionary);
                result = item;
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties, Dictionary<string, string> _dictionary) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[_dictionary[property.Name]].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (_dictionary.ContainsKey(property.Name))
                        if (row[_dictionary[property.Name]] == DBNull.Value)
                            property.SetValue(item, null, null);
                        else
                            property.SetValue(item, row[_dictionary[property.Name]], null);
                }
            }
            return item;
        }
        #endregion
    }
}
