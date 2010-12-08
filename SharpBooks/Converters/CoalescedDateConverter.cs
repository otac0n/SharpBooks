using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SharpBooks.Converters
{
    class CoalescedDateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(string)))
            {
                throw new NotImplementedException();
            }

            var firstDate = (from v in values
                             where v != null
                             select (DateTime)v).First();

            return firstDate.ToString("yyyy-MM-dd");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
