using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class StatusToColorConverter : IValueConverter, IMultiValueConverter
	{
		#region IValueConverter implement
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
		#endregion

		#region IMultiValueConverter implement
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			FrameworkElement targetElement = values[0] as FrameworkElement;
			string testCaseResult = values[1] as string;

			string brashStyleName = "";

			switch (testCaseResult)
			{
				case "PASS":
					brashStyleName = "wizardGreeBrush";
					break;
				case "FAIL":
					brashStyleName = "wizardRedBrush";
					break;
				default:
					brashStyleName = "wizardYellowBrush";
					break;
			}

			var newStyle = targetElement.TryFindResource(brashStyleName);

			return newStyle;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
