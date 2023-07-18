using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TestRunXMLParserTool.Converters
{
	class ToggleSwitchBorderColorConverter : IValueConverter
	{
		#region Fields
		private readonly string enabledBorderColor = "#0067c0";
		private readonly string disabledBorderColor = Colors.White.ToString();
		#endregion

		#region Implement IValueConverter
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return disabledBorderColor;
				}
				bool.TryParse(value.ToString(), out var enabled);
				if (enabled)
				{
					return enabledBorderColor;
				}
				else
				{
					return disabledBorderColor;
				}
			}
			catch (Exception)
			{
				return disabledBorderColor;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
