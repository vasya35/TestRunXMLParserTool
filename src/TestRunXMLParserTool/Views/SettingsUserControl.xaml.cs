using System.Diagnostics;
using System.Windows.Controls;

namespace TestRunXMLParserTool.Views
{
	/// <summary>
	/// Interaction logic for SettingsUserControl.xaml
	/// </summary>
	public partial class SettingsUserControl : UserControl
	{
		public SettingsUserControl()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
		}

    }
}
