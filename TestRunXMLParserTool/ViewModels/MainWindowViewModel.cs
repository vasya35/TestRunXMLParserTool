using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
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
			passedSelected = false;
			failedSelected = false;
			skippedSelected = false;
			sortSelected = false;
			genXMLCommand = new XMLGeneratorCommand();
			genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();

			OpenFileDialog openFileDialog = new()
			{
				FileName = "testng-results",
				DefaultExt = "xml",
				Filter = "Text documents (.xml)|*.xml"
			};

			bool? result = openFileDialog.ShowDialog();

			if (result == true)
			{
				OriginalTestCaseResults = XMLParserModel.Parse(openFileDialog.FileName);
				UpdateCounts();
			}

			DisplayedTestCaseResults = OriginalTestCaseResults;			

			PassedSelected = true;
			FailedSelected = true;
			SkippedSelected = true;
		}
		#endregion

		#region fields
		private TestCaseResultModel selectedTestCaseResult { get; set; }
		private ObservableCollection<TestCaseResultModel> displayedTestCaseResults { get; set; }
		private bool passedSelected { get; set; }
		private bool failedSelected { get; set; }
		private bool skippedSelected { get; set; }
		private bool sortSelected { get; set; }
		private string selectedPath { get; set; }
		private int passedCount { get; set; }
		private int failedCount { get; set; }
		private int skippedCount { get; set; }
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
				updateFilteredAndSortData(true);
				OnPropertyChanged("PassedSelected");
			}
		}

		public bool FailedSelected
		{
			get { return failedSelected; }
			set
			{
				failedSelected = value;
				updateFilteredAndSortData(true);
				OnPropertyChanged("FailedSelected");
			}
		}

		public bool SkippedSelected
		{
			get { return skippedSelected; }
			set
			{
				skippedSelected = value;
				updateFilteredAndSortData(true);
				OnPropertyChanged("SkippedSelected");
			}
		}

		public bool SortSelected
		{
			get { return sortSelected; }
			set
			{
				sortSelected = value;
				updateFilteredAndSortData(false);
				OnPropertyChanged("SortSelected");
			}
		}		
				
		public string SelectedPath
		{
			get { return selectedPath; }
			set
			{
				selectedPath = value;
				ReInitialisation();
				OnPropertyChanged("SelectedPath");
			}
		}

		public int PassedCount
		{
			get { return passedCount; }
			set
			{
				passedCount = value;
				OnPropertyChanged("PassedCount");
			}
		}

		public int FailedCount
		{
			get { return failedCount; }
			set
			{
				failedCount = value;
				OnPropertyChanged("FailedCount");
			}
		}

		public int SkippedCount
		{
			get { return skippedCount; }
			set
			{
				skippedCount = value;
				OnPropertyChanged("SkippedCount");
			}
		}
		#endregion

		#region implementation ICommand
		private ICommand genXMLCommand;
		private ICommand genJQueryScriptCommand;
		private ICommand openXMLCommand;
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

		public ICommand OpenXMLCommand
		{
			get
			{
				if (openXMLCommand == null)
					openXMLCommand = new RelayCommand(ExecuteOpenFileDialog);
				return openXMLCommand;
			}
		}
		#endregion

		#region Implementation INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

		#region Private methods
		private void ReInitialisation()
		{
			if (SelectedPath == "") return;
			
			selectedTestCaseResult = new TestCaseResultModel();
			displayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
			passedSelected = false;
			failedSelected = false;
			skippedSelected = false;
			sortSelected = false;
			genXMLCommand = new XMLGeneratorCommand();
			genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();
			OriginalTestCaseResults = XMLParserModel.Parse(SelectedPath);

			DisplayedTestCaseResults = OriginalTestCaseResults;

			PassedSelected = true;
			FailedSelected = true;
			SkippedSelected = true;			
		}

		private static ObservableCollection<TestCaseResultModel> sortData(ObservableCollection<TestCaseResultModel> filteredData)
		{
			return new ObservableCollection<TestCaseResultModel>(filteredData.OrderBy(x => x.getTestCaseNumber()));
		}

		private void updateFilteredAndSortData(bool isSelectedEnabled)
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

			ObservableCollection<TestCaseResultModel> filteredData = new ObservableCollection<TestCaseResultModel>();
			if (OriginalTestCaseResults != null)
			{
				filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.Where(x => filteredStatus.Contains(x.Result) == true).ToList());
			}

			if (isSelectedEnabled)
			{
				foreach (var item in filteredData) item.IsSelected = true;
			}

			if (sortSelected)
			{
				DisplayedTestCaseResults = sortData(filteredData);
			}
			else
			{
				DisplayedTestCaseResults = filteredData;
			}
		}

		private void ExecuteOpenFileDialog()
		{
			var dialog = new OpenFileDialog { };
			dialog.ShowDialog();

			SelectedPath = dialog.FileName;
		}

		private void UpdateCounts()
		{
			PassedCount = OriginalTestCaseResults.Where(x => x.Result == "PASS").ToList().Count();
			FailedCount = OriginalTestCaseResults.Where(x => x.Result == "FAIL").ToList().Count();
			SkippedCount = OriginalTestCaseResults.Where(x => x.Result == "SKIP").ToList().Count();
		}

		#endregion
	}
}
