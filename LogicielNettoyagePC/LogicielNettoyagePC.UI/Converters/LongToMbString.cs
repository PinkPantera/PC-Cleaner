using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace LogicielNettoyagePC.UI.Converters
{
    public class LongToMbString : MarkupExtension, IValueConverter
    {
        private const long OneMb = 1024 * 1024;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is long))
                return String.Empty;

            var result = Math.Round((double)(long)value / OneMb, 3);

            return  $"{result.ToString()} Mb";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
