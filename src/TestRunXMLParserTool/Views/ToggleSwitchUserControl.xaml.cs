using System.Windows.Controls;
using System.Windows.Input;

namespace TestRunXMLParserTool.Views
{
	/// <summary>
	/// Interaction logic for ToggleSwitchUserControl.xaml
	/// </summary>
	public partial class ToggleSwitchUserControl : UserControl
	{
		#region Fileds
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		#endregion

		public ToggleSwitchUserControl()
		{
			InitializeComponent();
		}

		private void SwitchByMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				DataContext = !(bool)DataContext;
			}
			catch (System.Exception error)
			{
				Logger.Warn($"DataContext for ToggleSwitchUserControl wasn't set: {error}");
			}
		}
	}
}
