using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			DataContext = new MainWindowViewModel();
		}
	}
}
