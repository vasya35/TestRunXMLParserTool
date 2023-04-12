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
			return data switch
			{
				"PASS" => "LightGreen",
				"FAIL" => "IndianRed",
				_ => "Yellow",
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string data = (string)value;
			return data switch
			{
				"LightGreen" => "PASS",
				"IndianRed" => "FAIL",
				_ => "SKIP",
			};			
		}
	}
}
