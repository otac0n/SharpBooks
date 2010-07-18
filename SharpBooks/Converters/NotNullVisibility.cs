namespace SharpBooks.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class NotNullVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
            {
                throw new NotImplementedException();
            }

            return object.ReferenceEquals(value, null) ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
