using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindowView : Window
	{
		public MainWindowView()
		{
			InitializeComponent();

			DataContext = new MainWindowViewModel();
		}
	}
}
