using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool.Views;

public partial class SettingsWindowView : Window
{
	public SettingsWindowView(SettingsViewModel settingsViewModel)
	{
		InitializeComponent();

		DataContext = settingsViewModel;
	}
}
