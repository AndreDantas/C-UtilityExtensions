using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Extensions
{
    public static class QueueExtensions
    {
        public static T SafePeek<T>(this Queue<T> q)
        {
            if (q.Count > 0)
                return q.Peek();

            return default;
        }

        public static T LastOrDefault<T>(this Queue<T> q, T @default)
        {
            if (q.Count > 0)
                return q.LastOrDefault() ?? @default;

            return @default;
        }
    }
}