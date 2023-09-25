using System;
using System.Globalization;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters;

[ValueConversion(typeof(string), typeof(string))]
internal class SplitTestsLabelConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			return $"{values[0]}/{values[1]}";
		}
		catch (Exception)
		{
			return "";
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
