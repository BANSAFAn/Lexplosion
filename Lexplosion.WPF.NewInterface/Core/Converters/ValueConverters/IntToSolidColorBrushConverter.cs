using System;
using System.Globalization;
using System.Windows.Media;

namespace Lexplosion.WPF.NewInterface.Core.Converters
{
    public sealed class IntToSolidColorBrushConverter : ConverterBase<IntToSolidColorBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intHexColor) 
            {
                byte r = (byte)((intHexColor >> 16) & 0xFF);
                byte g = (byte)((intHexColor >> 8) & 0xFF);
                byte b = (byte)(intHexColor & 0xFF);

                return new SolidColorBrush(Color.FromArgb(255, r, g, b));
            }

            if (value is uint uIntHexColor)
            {
                byte a = (byte)((uIntHexColor >> 24) & 0xFF);
                byte r = (byte)((uIntHexColor >> 16) & 0xFF);
                byte g = (byte)((uIntHexColor >> 8) & 0xFF);
                byte b = (byte)(uIntHexColor & 0xFF);

                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }

            throw new ArgumentException($"value must be integer not {value.GetType()}");
        }
    }
}
