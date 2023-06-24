using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TestRunXMLParserTool.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class ActivateToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return Colors.Green.ToString();
			}
			else return Colors.Transparent.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
