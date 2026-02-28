using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1.Converters
{
    public class AccentBrushConverter : IValueConverter
    {
        // parameter = opacity byte 0-255
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color c)
            {
                byte alpha = 255;
                if (parameter is string s && byte.TryParse(s, out byte a))
                    alpha = a;
                return new SolidColorBrush(Color.FromArgb(alpha, c.R, c.G, c.B));
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
