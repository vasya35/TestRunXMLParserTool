using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TestRunXMLParserTool.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class StatusToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string date = (string)value;
			switch (date)
			{
				case "PASS":
					return "Green";
				case "FAIL":
					return "Red";
				default:
					return "Yellow";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
