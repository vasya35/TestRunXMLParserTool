using System.Windows;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		App()
		{
			setCurrentCulture();
		}

		private void setCurrentCulture()
		{
			var culture = AppConfiguration.GetCurrentCulture();

			if (culture != null)
			{
				System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
			}
		}
	}
}
