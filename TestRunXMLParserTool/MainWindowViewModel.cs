using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestRunXMLParserTool.Commands;

namespace TestRunXMLParserTool
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region .ctor

		public MainWindowViewModel()
		{
			var test = new XMLParser();
			OriginalTestCaseResults = test.Parse("testng-results_tkfd.xml");
			DisplayedTestCaseResults = OriginalTestCaseResults;
			passedSelected = true;
			failedSelected = true;
			skippedSelected = true;
			sortSelected = false;
		}

		#endregion

		#region fields
		private TestCaseResultModel selectedTestCaseResult { get; set; }
		private ObservableCollection<TestCaseResultModel> displayedTestCaseResults { get; set; }
		private bool passedSelected { get; set; }
		private bool failedSelected { get; set; }
		private bool skippedSelected { get; set; }
		private bool sortSelected { get; set; }

		private ICommand genXMLCommand;
		public ICommand GenXMLCommand
		{
			get
			{
				if (genXMLCommand == null)
					genXMLCommand = new GenerateXMLCommand();
				return genXMLCommand;
			}
		}
		#endregion

		#region Properties
		public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; }
		public ObservableCollection<TestCaseResultModel> DisplayedTestCaseResults 
		{ 
			get { return displayedTestCaseResults; }
			set
			{
				displayedTestCaseResults = value;
				OnPropertyChanged("DisplayedTestCaseResults");
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
				updateFilteredAndSortData();
				OnPropertyChanged("PassedSelected");
			}
		}

		public bool FailedSelected
		{
			get { return failedSelected; }
			set
			{
				failedSelected = value;
				updateFilteredAndSortData();
				OnPropertyChanged("FailedSelected");
			}
		}

		public bool SkippedSelected
		{
			get { return skippedSelected; }
			set
			{
				skippedSelected = value;
				updateFilteredAndSortData();
				OnPropertyChanged("SkippedSelected");
			}
		}

		public bool SortSelected
		{
			get { return sortSelected; }
			set
			{
				sortSelected = value;
				updateFilteredAndSortData();
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
		
		private ObservableCollection<TestCaseResultModel> sortData(ObservableCollection<TestCaseResultModel> filteredData)
		{
			return new ObservableCollection<TestCaseResultModel>(filteredData.OrderBy(x => x.getTestCaseNumber()));
		}


		private void updateFilteredAndSortData()
		{
			List<string> filteredStatus = new List<string>();

			if (PassedSelected)
			{
				filteredStatus.Add(new string("PASS"));
			}

			if (FailedSelected)
			{
				filteredStatus.Add(new string("FAIL"));
			}

			if (SkippedSelected)
			{
				filteredStatus.Add(new string("SKIP"));
			}

			var filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.Where(x => filteredStatus.Contains(x.Result)==true).ToList());

			if (sortSelected)
			{
				DisplayedTestCaseResults = sortData(filteredData);
			}
			else
			{
				DisplayedTestCaseResults = filteredData;
			}
		}

		#endregion
	}
}
