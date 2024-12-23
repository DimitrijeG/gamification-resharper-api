using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.UIInteractive.Wpf.Converters;

public class ProgressToRectangleGeometryConverter : IMultiValueConverter
{
  public static readonly ProgressToRectangleGeometryConverter Instance = new();

  private ProgressToRectangleGeometryConverter()
  {
  }

  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 3 ||
        values[0] is not double progress ||
        values[1] is not double width ||
        values[2] is not double height) return DependencyProperty.UnsetValue;

    var clipHeight = Math.Min(height, height * (progress / 100.0));
    return new RectangleGeometry(new Rect(0, height - clipHeight, width, clipHeight));
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}