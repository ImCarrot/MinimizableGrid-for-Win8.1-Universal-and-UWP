using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MinimizableGrid.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool IsInverseReq { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsInverseReq)
            {
                var val = (bool)value;
                if (val)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
            else
            {
                var val = (bool)value;
                if (val)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
