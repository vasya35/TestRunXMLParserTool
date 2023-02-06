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
			OriginalTestCaseResults = test.Parse("testng-results_tkfd.xml");
			FilteredTestCaseResults = OriginalTestCaseResults;
			passedSelected = true;
			failedSelected = true;
			undefinedSelected = true;
			sortSelected = false;
		}

		#endregion

		#region fields
		private TestCaseResultModel selectedTestCaseResult { get; set; }
		private ObservableCollection<TestCaseResultModel> filteredTestCaseResults { get; set; }
		private bool passedSelected { get; set; }
		private bool failedSelected { get; set; }
		private bool undefinedSelected { get; set; }
		private bool sortSelected { get; set; }
		#endregion

		#region Properties
		public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; }
		public ObservableCollection<TestCaseResultModel> FilteredTestCaseResults 
		{ 
			get { return filteredTestCaseResults; }
			set
			{
				filteredTestCaseResults = value;
				OnPropertyChanged("FilteredTestCaseResults");
			} 
		}

		public TestCaseResultModel SelectedTestCaseResult
		{
			get { return selectedTestCaseResult; }
			set
			{
				selectedTestCaseResult = value;
				OnPropertyChanged("SelectedTestCaseResult");
			}
		}

		public bool PassedSelected
		{
			get { return passedSelected; }
			set
			{
				passedSelected = value;
				OnPropertyChanged("PassedSelected");
			}
		}

		public bool FailedSelected
		{
			get { return failedSelected; }
			set
			{
				failedSelected = value;
				OnPropertyChanged("FailedSelected");
			}
		}

		public bool UndefinedSelected
		{
			get { return undefinedSelected; }
			set
			{
				undefinedSelected = value;
				OnPropertyChanged("UndefinedSelected");
			}
		}

		public bool SortSelected
		{
			get { return sortSelected; }
			set
			{
				sortSelected = value;
				if (value)
				{
					sortData();
				}
				else
				{
					unsortData();
				}
				OnPropertyChanged("SortSelected");				
			}
		}		
		#endregion

		#region Implementation INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		#region Private methods
		
		private void sortData()
		{
			FilteredTestCaseResults = new ObservableCollection<TestCaseResultModel>(OriginalTestCaseResults.OrderBy(x => x.getTestCaseNumber()));
		}

		private void unsortData()
		{
			FilteredTestCaseResults = OriginalTestCaseResults;
		}

		#endregion
	}
}
