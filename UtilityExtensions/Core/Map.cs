using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UtilityExtensions.Extensions;

namespace UtilityExtensions.Core
{
    //TODO: Handle list of complex objects

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MapIgnore : Attribute
    {
    }

    /// <summary>
    /// Class that represents the json structure.
    /// </summary>
    public sealed class Map : Dictionary<string, object>, ICloneable
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Map otherMap)
            {
                if (this.Keys.Count != otherMap.Keys.Count)
                {
                    return false;
                }

                foreach (KeyValuePair<string, object> item in this)
                {
                    if (!otherMap.ContainsKey(item.Key))
                    {
                        return false;
                    }

                    if (!CompareObjects(item.Value, otherMap[item.Key]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 340188902;
            hashCode = hashCode * -1521134295 + EqualityComparer<KeyCollection>.Default.GetHashCode(Keys);
            hashCode = hashCode * -1521134295 + EqualityComparer<ValueCollection>.Default.GetHashCode(Values);
            return hashCode;
        }

        public static bool operator ==(Map left, Map right)
        {
            return EqualityComparer<Map>.Default.Equals(left, right);
        }

        public static bool operator !=(Map left, Map right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            StringBuilder sr = new();
            sr.Append('{');
            int keyCount = Keys.Count;
            using (CultureChanger cc = new(CultureInfo.InvariantCulture))
            {
                foreach (KeyValuePair<string, object> item in this)
                {
                    keyCount--;

                    sr.Append($"\"{item.Key}\":");
                    sr.Append(ConvertObjectToString(item.Value));

                    if (keyCount > 0)
                    {
                        sr.Append(',');
                    }
                }
            }
            sr.Append('}');
            return sr.ToString();
        }

        private class MapObjectComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                return CompareObjects(x, y);
            }

            public int GetHashCode(object obj)
            {
                return obj?.GetHashCode() ?? 0;
            }
        }

        private static bool CompareObjects(object a, object b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a?.GetType() != b?.GetType())
            {
                return false;
            }

            if (a.GetType().IsIEnumerable() && a is Map == false)
            {
                return CompareIEnumerable(a as IEnumerable, b as IEnumerable);
            }
            else
            {
                return a.Equals(b);
            }
        }

        private static bool CompareIEnumerable(IEnumerable a, IEnumerable b)
        {
            return ListExtensions.EqualsSequenceIgnoreOrder(a, b, new MapObjectComparer());
        }

        private static string ConvertObjectToString(object obj)
        {
            StringBuilder sr = new();

            if (obj == null)
            {
                sr.Append("null");
            }
            else if (obj is string)
            {
                sr.Append($"\"{obj}\"");
            }
            else if (obj.GetType().IsIEnumerable() && obj is Map == false)
            {
                sr.Append(ConvertIEnumerableToString(obj as IEnumerable));
            }
            else
            {
                sr.Append(obj.ToString());
            }

            return sr.ToString();
        }

        private static string ConvertIEnumerableToString(IEnumerable obj)
        {
            if (obj == null)
            {
                return "null";
            }

            StringBuilder sr = new();
            sr.Append('[');

            foreach (object item in obj)
            {
                sr.Append(ConvertObjectToString(item));
                sr.Append(',');
            };
            if (sr.Length > 1)
            {
                sr.Length--;
            }

            sr.Append(']');

            return sr.ToString();
        }

        public object Clone()
        {
            Map clone = new();

            foreach (string key in Keys)
            {
                clone[key] = this[key] switch
                {
                    ICloneable cloneable => cloneable.Clone(),
                    _ => this[key],
                };
            }

            return clone;
        }
    }

    public static class MapExtensions
    {
        /// <summary>
        /// Converts this object to a <seealso cref="Map" />.
        /// </summary>
        /// <remarks>
        /// Properties with the <seealso cref="MapIgnore" /> attribute or of type <seealso
        /// cref="Map" />, will be ignored.
        /// </remarks>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static Map ToMap(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            System.Reflection.PropertyInfo[] props = obj.GetType().GetProperties();

            Map map = new();

            foreach (System.Reflection.PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(Map))
                {
                    continue;
                }

                object[] attrs = prop.GetCustomAttributes(typeof(MapIgnore), true);
                if (attrs.Length == 1)
                {
                    continue;
                }

                if (prop.PropertyType.IsSimpleType() || prop.PropertyType.IsIEnumerable())
                {
                    map[prop.Name] = prop.GetValue(obj);
                }
                else
                {
                    map[prop.Name] = prop.GetValue(obj).ToMap();
                }
            }

            return map;
        }

        /// <summary>
        /// Uses this <seealso cref="Map" /> to create a new instance of <typeparamref name="T" />.
        /// </summary>
        /// <remarks>
        /// Properties with the <seealso cref="MapIgnore" /> attribute or of type <seealso
        /// cref="Map" />, will be ignored.
        /// </remarks>
        /// <typeparam name="T"> </typeparam>
        /// <param name="map"> </param>
        /// <returns> </returns>
        public static T FromMap<T>(this Map map) where T : class, new()
        {
            T obj = new();
            Type objType = obj.GetType();

            foreach (KeyValuePair<string, object> item in map)
            {
                System.Reflection.PropertyInfo prop = objType.GetProperty(item.Key);

                if (prop == null || prop.PropertyType == typeof(Map))
                {
                    continue;
                }

                object[] attrs = prop.GetCustomAttributes(typeof(MapIgnore), true);
                if (attrs.Length == 1)
                {
                    continue;
                }

                if (item.Value is Map)
                {
                    System.Reflection.MethodInfo mi = typeof(MapExtensions).GetMethod("FromMap");
                    System.Reflection.MethodInfo miRef = mi.MakeGenericMethod(prop.PropertyType);
                    prop.SetValue(obj, miRef.Invoke(null, new object[1] { item.Value }), null);
                }
                else
                {
                    prop.SetValue(obj, item.Value.ConvertTo(prop.PropertyType), null);
                }
            }

            return obj;
        }
    }
}