using System;
using System.Globalization;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters;

[ValueConversion(typeof(int), typeof(int))]
public class SelectingGridLabeMinWidthConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value.ToString().Length * 2 + 1) * 10;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
