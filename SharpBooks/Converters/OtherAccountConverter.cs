using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SharpBooks.Converters
{
    public class OtherAccountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var split = value as Split;
            if (split == null)
            {
                throw new ArgumentException("value");
            }

            var transaction = split.Transaction;

            if (transaction.Splits.Count() != 2)
            {
                return "(Split)";
            }

            var otherAccount = transaction.Splits.Where(s => s.Account != split.Account).Select(s => s.Account).ToList();
            if (otherAccount.Count != 1)
            {
                return "(Split)";
            }

            return otherAccount[0].Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
