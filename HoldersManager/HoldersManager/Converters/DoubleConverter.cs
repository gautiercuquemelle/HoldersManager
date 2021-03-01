using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HoldersManager.Converters
{
    public class DoubleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            double doubleValue = (double)value;
            return doubleValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                strValue = "0";
            double resultdouble;
            if (double.TryParse(strValue, out resultdouble))
            {
                return resultdouble;
            }
            return 0;
        }

    }
}
