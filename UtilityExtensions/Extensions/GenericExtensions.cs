using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UtilityExtensions.Extensions
{
    public static class GenericExtensions
    {
        public static bool In<T>(this T obj, params T[] collection)
        {
            return obj.In(null, collection);
        }

        public static bool In<T>(this T obj, Func<T, T, bool> comparer, params T[] collection)
        {
            if (collection == null)
            {
                return false;
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default.Equals;
            }

            for (int i = 0; i < collection.Length; i++)
            {
                if (comparer(obj, collection[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to convert this object to an object of the given Type
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="destinationType"> </param>
        /// <returns> </returns>
        public static object ConvertTo(this object source, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (destinationType.IsGenericType &&
                destinationType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (source == null)
                {
                    return null;
                }
                destinationType = Nullable.GetUnderlyingType(destinationType);
            }

            return System.Convert.ChangeType(source, destinationType);
        }

        public static string GetDescription(this Enum enumValue)
        {
            return ((DescriptionAttribute)Attribute.GetCustomAttribute((enumValue.GetType().GetField(enumValue.ToString())), typeof(DescriptionAttribute))).Description;
        }
    }
}