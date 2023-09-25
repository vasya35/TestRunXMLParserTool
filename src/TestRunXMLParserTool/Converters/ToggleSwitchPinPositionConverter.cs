using System;
using System.Globalization;
using System.Windows.Data;

namespace TestRunXMLParserTool.Converters;

class ToggleSwitchPinPositionConverter : IValueConverter
{
	#region Fields
	private const int disablePinPosition = 0;
	private const int enablePinPosition = 2;
	#endregion

	#region Implement IValueConverter
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (value == null)
			{
				return disablePinPosition;
			}
			bool.TryParse(value.ToString(), out var enabled);
			if (enabled)
			{
				return enablePinPosition;
			}
			else
			{
				return disablePinPosition;
			}
		}
		catch (Exception)
		{
			return disablePinPosition;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
	#endregion
}
