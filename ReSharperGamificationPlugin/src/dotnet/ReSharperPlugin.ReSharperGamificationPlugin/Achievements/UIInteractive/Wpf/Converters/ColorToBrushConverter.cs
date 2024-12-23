using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive.Wpf.Converters;

public class ColorToBrushConverter : IValueConverter
{
  public static readonly ColorToBrushConverter Instance = new();

  private ColorToBrushConverter()
  {
  }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is Color color ? new SolidColorBrush(color) : new SolidColorBrush();
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is SolidColorBrush solidColorBrush) return solidColorBrush.Color;
    return Colors.Black;
  }
}
