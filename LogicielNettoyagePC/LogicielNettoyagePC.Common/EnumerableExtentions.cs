using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.Common
{
    public static class EnumerableExtentions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}
