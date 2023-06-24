using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool.Views
{
	/// <summary>
	/// Interaction logic for SettingsWindowView.xaml
	/// </summary>
	public partial class SettingsWindowView : Window
	{
		public SettingsWindowView(SettingsViewModel settingsViewModel)
		{
			InitializeComponent();

			DataContext = settingsViewModel;
		}

	}
}
