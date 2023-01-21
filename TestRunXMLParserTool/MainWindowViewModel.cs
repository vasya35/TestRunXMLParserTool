using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestRunXMLParserTool
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region .ctor

		public MainWindowViewModel()
		{
			var test = new XMLParser();
			TestCaseResults = test.Parse("testng-results_tkfd.xml");
		}

		#endregion

		private TestCaseResultModel selectedTestCaseResult { get; set; }

		public ObservableCollection<TestCaseResultModel> TestCaseResults { get; set; }
		public TestCaseResultModel SelectedTestCaseResult
		{
			get { return selectedTestCaseResult; }
			set
			{
				selectedTestCaseResult = value;
				OnPropertyChanged("SelectedTestCaseResult");
			}
		}


		#region Implementation INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion
	}
}
