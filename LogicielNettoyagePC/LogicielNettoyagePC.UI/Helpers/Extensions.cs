using LogicielNettoyagePC.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.UI.Helpers
{
    public static class Extensions
    {
        private static int[] pageKindValuesSet = (int[])Enum.GetValues(typeof(PageKind));

        public static PageKind ConvertToPageKind(this string value)
        {
            PageKind result;
            return (Enum.TryParse(value, out result) && Array.BinarySearch(pageKindValuesSet, (int)result) >= 0)
                ? result
                : PageKind.Unknown;
        }
    }
}
