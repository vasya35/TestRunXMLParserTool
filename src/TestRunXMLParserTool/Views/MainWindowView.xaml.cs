using System.Windows;
using TestRunXMLParserTool.ViewModels;

namespace TestRunXMLParserTool;

public partial class MainWindowView : Window
{
	private MainWindowViewModel mainWindowViewModel;
	public MainWindowView(MainWindowViewModel mainWindowViewModel)
	{
		InitializeComponent();

		this.mainWindowViewModel = mainWindowViewModel;

		DataContext = mainWindowViewModel;
	}
}
