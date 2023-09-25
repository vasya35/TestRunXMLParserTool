using System.Diagnostics;
using System.Windows.Controls;

namespace TestRunXMLParserTool.Views;

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
