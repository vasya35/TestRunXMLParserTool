using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TestRunXMLParserTool.Converters;

class ToggleSwitchPinColorConverter : IValueConverter
{
	#region Fields
	private readonly string enabledPinColor = Colors.White.ToString();
	private readonly string disabledPinColor = Colors.Gray.ToString();
	#endregion

	#region Implement IValueConverter
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		
		try
		{
			if (value == null) { return disabledPinColor; }
			bool.TryParse(value.ToString(), out var enabled);
			if (enabled)
			{
				return enabledPinColor;
			}
			else
			{
				return disabledPinColor;
			}
		}
		catch (Exception)
		{
			return disabledPinColor;
		}			
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
	#endregion
}
