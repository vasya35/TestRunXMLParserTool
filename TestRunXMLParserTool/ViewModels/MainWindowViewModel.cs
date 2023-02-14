using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestRunXMLParserTool.Commands;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region .ctor
		public MainWindowViewModel()
		{
			selectedTestCaseResult = new TestCaseResultModel();
			displayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
			passedSelected = true;
			failedSelected = true;
			skippedSelected = true;
			sortSelected = false;
			genXMLCommand = new XMLGeneratorCommand();
			genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();

			var testCase = new XMLParserModel();
			OriginalTestCaseResults = testCase.Parse("testng-results_tkfd.xml");
			DisplayedTestCaseResults = OriginalTestCaseResults;
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
		private ICommand genJQueryScriptCommand;

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

		public ICommand GenXMLCommand
		{
			get
			{
				if (genXMLCommand == null)
					genXMLCommand = new XMLGeneratorCommand();
				return genXMLCommand;
			}
		}

		public ICommand GenJQueryScriptCommand
		{
			get
			{
				if (genJQueryScriptCommand == null)
					genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();
				return genJQueryScriptCommand;
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

		private static ObservableCollection<TestCaseResultModel> sortData(ObservableCollection<TestCaseResultModel> filteredData)
		{
			return new ObservableCollection<TestCaseResultModel>(filteredData.OrderBy(x => x.getTestCaseNumber()));
		}


		private void updateFilteredAndSortData()
		{
			List<string> filteredStatus = new();

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

			var filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.Where(x => filteredStatus.Contains(x.Result) == true).ToList());

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
