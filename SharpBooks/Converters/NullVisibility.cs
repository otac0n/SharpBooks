namespace SharpBooks.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class NullVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(Visibility)))
            {
                throw new NotImplementedException();
            }

            return object.ReferenceEquals(value, null) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
