using System.Threading;
using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool.Views
{
	/// <summary>
	/// Interaction logic for SettingsWindowView.xaml
	/// </summary>
	public partial class SettingsWindowView : Window
	{
		public SettingsWindowView()
		{
			InitializeComponent();

			DataContext = new SettingsViewModel();
		}
	}
}
