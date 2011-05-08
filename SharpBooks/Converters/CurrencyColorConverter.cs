﻿namespace SharpBooks.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Media;

    public class CurrencyColorConverter : IValueConverter
    {
        private static readonly Brush red = new SolidColorBrush(Colors.Red);
        private static readonly Brush black = new SolidColorBrush(Colors.Black);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(Brush)))
            {
                throw new NotImplementedException();
            }

            return (long)value < 0 ? red : black;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}