using System.Configuration;
using System.Windows;

namespace TestRunXMLParserTool
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		App()
		{
			// Supported: en-US, ru-RU, kk-KZ, tr-TR
			var culture = ConfigurationManager.AppSettings["Culture"];
			if (culture != null) {
				System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
			}
		}
	}
}
