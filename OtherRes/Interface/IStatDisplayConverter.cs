using Microsoft.Toolkit.Uwp.UI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MSEACalculator.OtherRes.Interface
{
    public class IStatDisplayConverter : IValueConverter
    {
        //public int cValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            char[] toTrim = { '+', '%', ' ', ':' };
            int nValue = int.Parse(value.ToString().Trim(toTrim));

            if(nValue == 0)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
