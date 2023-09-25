using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters;

[ValueConversion(typeof(string), typeof(string))]
public class VisibilityStepLeftPathConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if ((bool)values[0] == true)
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

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
