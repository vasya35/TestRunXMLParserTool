using System;
using System.Globalization;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class StatusToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string data = (string)value;
			switch (data)
			{
				case "PASS":
					return "LightGreen";
				case "FAIL":
					return "IndianRed";
				default:
					return "Yellow";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string data = (string)value;
			switch (data)
			{
				case "LightGreen":
					return "PASS";
				case "IndianRed":
					return "FAIL";
				default:
					return "SKIP";
			}
		}
	}
}
