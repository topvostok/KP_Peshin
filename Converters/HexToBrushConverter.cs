using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1.Converters
{
    /// <summary>РљРѕРЅРІРµСЂС‚РёСЂСѓРµС‚ hex-СЃС‚СЂРѕРєСѓ "#FFRRGGBB" РІ SolidColorBrush РґР»СЏ Р±РёРЅРґРёРЅРіР° С†РІРµС‚Р° СЃС‚Р°С‚СѓСЃР°.</summary>
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string hex)

            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(hex);
                    return new SolidColorBrush(color);
                }
                catch { }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
