using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Project.Chat
{
    public class BoolToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSentByCurrentUser)
            {
                return isSentByCurrentUser ? new Thickness(50, 0, 0, 0) : new Thickness(0, 0, 50, 0);
            }
            return new Thickness(0, 0, 50, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
