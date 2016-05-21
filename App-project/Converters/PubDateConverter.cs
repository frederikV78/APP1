using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace App_project.Converters
{
    class PubDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                return date.ToString("dddd d MMMM yyyy, HH:mm:ss");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                DateTime date = DateTime.Parse(value.ToString());
                return date;
            }
            return DateTime.Now;
        }
    }
}
