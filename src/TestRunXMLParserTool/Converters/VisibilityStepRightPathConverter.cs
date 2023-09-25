using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters;

[ValueConversion(typeof(string), typeof(string))]
internal class VisibilityStepRightPathConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if ((bool)values[1] == true)
			{
				return Visibility.Hidden;
			}
			else return Visibility.Visible;
		}
		catch (Exception)
		{
			return Visibility.Visible;
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
