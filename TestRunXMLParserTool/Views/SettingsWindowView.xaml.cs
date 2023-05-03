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

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Visibility = Visibility.Hidden;
			e.Cancel = true;
		}
	}
}
