using System;
using System.Globalization;

namespace UtilityExtensions.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static string TryToString(this DateTime dt, string format)
        {
            string result;
            try
            {
                result = dt.ToString(format);
            }
            catch
            {
                result = dt.ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static string DescriptiveTimeDifference(this DateTime now, DateTime other)
        {
            int timeDifference = (other.Date - now.Date).Days;
            if (timeDifference == 0)
            {
                return "Today";
            }

            if (timeDifference == 1)
            {
                return "Yesterday";
            }

            if (timeDifference == -1)
            {
                return "Tomorrow";
            }

            if (timeDifference > 0)
            {
                if (timeDifference > 30)
                {
                    return $"{(timeDifference / 30) } month{((timeDifference / 30) == 1 ? "" : "s")} ago";
                }
                else
                {
                    return $"{timeDifference} day{(timeDifference == 1 ? "" : "s")} ago";
                }
            }
            else
            {
                if (timeDifference < -30)
                {
                    return $"In {(timeDifference / 30)} month{((timeDifference / 30) == -1 ? "" : "s")}";
                }
                else
                {
                    return $"In {timeDifference} day{(timeDifference == -1 ? "" : "s")}";
                }
            }
        }
    }
}