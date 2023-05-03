using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using TestRunXMLParserTool.Commands;
using TestRunXMLParserTool.Models;
using TestRunXMLParserTool.Views;

namespace TestRunXMLParserTool.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region .ctor
		public MainWindowViewModel(MainWindowView mainWindowView)
		{
			selectedTestCaseResult = new TestCaseResultModel();
			displayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
			passedSelected = false;
			failedSelected = false;
			skippedSelected = false;
			sortSelected = false;
			genXMLCommand = new XMLGeneratorCommand();
			genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();

			steps = new List<StepDescription>() {
				new StepDescription()
				{
					Name = Properties.Resources.OpenFileStep,
					IsActivate = true,
					ActivateAction = new RelayCommand(Step1Activate),
					IsFirstStep = true,
					IsLastStep = false
				},
				new StepDescription()
				{
					Name = Properties.Resources.FilterAndSortStep,
					IsActivate = false,
					ActivateAction = new RelayCommand(Step2Activate),
					IsFirstStep = false,
					IsLastStep = false
				},
				new StepDescription()
				{
					Name = Properties.Resources.GenerateFileStep,
					IsActivate = false,
					ActivateAction = new RelayCommand(Step3Activate),
					IsFirstStep = false,
					IsLastStep = true
				}
			};

			ExecuteOpenFileDialog();
			this.mainWindowView = mainWindowView;
		}
		#endregion

		#region fields
		private TestCaseResultModel selectedTestCaseResult;
		private ObservableCollection<TestCaseResultModel> displayedTestCaseResults;
		private bool? passedSelected;
		private bool? failedSelected;
		private bool? skippedSelected;
		private bool sortSelected;
		private string selectedPath = "";
		private int passedCount;
		private int failedCount;
		private int skippedCount;
		private int passedSelectedCount;
		private int failedSelectedCount;
		private int skippedSelectedCount;
		private int currentStep;
		private List<StepDescription> steps;
		private MainWindowView mainWindowView;
		private SettingsWindowView settingsWindow;
		#endregion

		#region Properties
		public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; } = new();
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

		public bool? PassedSelected
		{
			get { return passedSelected; }
			set
			{
				passedSelected = value;
				updateFilteredAndSortData(true);
				OnPropertyChanged("PassedSelected");
			}
		}

		public bool? FailedSelected
		{
			get { return failedSelected; }
			set
			{
				failedSelected = value;
				updateFilteredAndSortData(true);
				OnPropertyChanged("FailedSelected");
			}
		}

		public bool? SkippedSelected
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

		public int PassedSelectedCount
		{
			get { return passedSelectedCount; }
			set
			{
				passedSelectedCount = value;
				OnPropertyChanged("PassedSelectedCount");
			}
		}

		public int FailedSelectedCount
		{
			get { return failedSelectedCount; }
			set
			{
				failedSelectedCount = value;
				OnPropertyChanged("FailedSelectedCount");
			}
		}

		public int SkippedSelectedCount
		{
			get { return skippedSelectedCount; }
			set
			{
				skippedSelectedCount = value;
				OnPropertyChanged("SkippedSelectedCount");
			}
		}

		public List<StepDescription> Steps
		{
			get { return steps; }
			set
			{
				steps = value;
				OnPropertyChanged("Steps");
			}
		}

		public int CurrentStep
		{
			get { return currentStep; }
			set
			{
				currentStep = value;
				OnPropertyChanged("CurrentStep");
			}
		}
		#endregion

		#region implementation ICommand
		private ICommand genXMLCommand;
		private ICommand genJQueryScriptCommand;
		private ICommand? openXMLCommand;
		private ICommand? settingsButtonCommand;

		public ICommand GenXMLCommand
		{
			get
			{
				genXMLCommand ??= new XMLGeneratorCommand();
				return genXMLCommand;
			}
		}

		public ICommand GenJQueryScriptCommand
		{
			get
			{
				genJQueryScriptCommand ??= new JSTestrailSelectorScriptGeneratorCommand();
				return genJQueryScriptCommand;
			}
		}

		public ICommand OpenXMLCommand
		{
			get
			{
				openXMLCommand ??= new RelayCommand(ExecuteOpenFileDialog);
				return openXMLCommand;
			}
		}

		public ICommand SettingsButtonCommand
		{
			get
			{
				settingsButtonCommand ??= new RelayCommand(OpenSettingsWindow);
				return settingsButtonCommand;
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

			foreach (var testCase in OriginalTestCaseResults)
			{
				testCase.SelectChangedNotify += UpdateSelectedCount;
			}

			Step2Activate();
			DisplayedTestCaseResults = OriginalTestCaseResults;

			UpdateCounts();

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
			List<string> changingStatus = new();

			if (PassedSelected != false)
			{
				filteredStatus.Add(new string("PASS"));
			}
			if (PassedSelected == true)
			{
				changingStatus.Add(new string("PASS"));
			}

			if (FailedSelected != false)
			{
				filteredStatus.Add(new string("FAIL"));
			}
			if (FailedSelected == true)
			{
				changingStatus.Add(new string("FAIL"));
			}

			if (SkippedSelected != false)
			{
				filteredStatus.Add(new string("SKIP"));
			}
			if (SkippedSelected == true)
			{
				changingStatus.Add(new string("SKIP"));
			}

			ObservableCollection<TestCaseResultModel> filteredData = new();
			if (OriginalTestCaseResults != null)
			{
				filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.Where(x => filteredStatus.Contains(x.Result) == true).ToList());
			}

			if (sortSelected)
			{
				DisplayedTestCaseResults = sortData(filteredData);
			}
			else
			{
				DisplayedTestCaseResults = filteredData;
			}

			if (isSelectedEnabled)
			{
				foreach (var item in filteredData)
				{
					foreach (var status in changingStatus)
					{
						if (item.Result == status)
						{
							item.IsSelected = true;
						}
					}
				}
			}
			UpdateSelectedCount();
		}

		private void ExecuteOpenFileDialog()
		{
			var dialog = new OpenFileDialog { };
			dialog.ShowDialog();

			SelectedPath = dialog.FileName;
		}

		private void OpenSettingsWindow()
		{
			settingsWindow = new SettingsWindowView();
			settingsWindow.Owner = mainWindowView;
			settingsWindow.Show();
		}

		private void UpdateCounts()
		{
			PassedCount = OriginalTestCaseResults.Where(x => x.Result == "PASS").ToList().Count;
			FailedCount = OriginalTestCaseResults.Where(x => x.Result == "FAIL").ToList().Count;
			SkippedCount = OriginalTestCaseResults.Where(x => x.Result == "SKIP").ToList().Count;
		}

		private void Step1Activate()
		{
			steps[0].IsActivate = true;
			steps[0].IsNextAcvtive = false;

			steps[1].IsActivate = false;
			steps[1].IsNextAcvtive = false;

			steps[2].IsActivate = false;
			CurrentStep = 0;
		}

		private void Step2Activate()
		{
			steps[0].IsActivate = true;
			steps[0].IsNextAcvtive = true;

			steps[1].IsActivate = true;
			steps[1].IsNextAcvtive = false;

			steps[2].IsActivate = false;
			steps[2].IsNextAcvtive = false;
			CurrentStep = 1;
		}

		private void Step3Activate()
		{
			steps[0].IsActivate = true;
			steps[0].IsNextAcvtive = true;

			steps[1].IsNextAcvtive = true;
			steps[1].IsActivate = true;

			steps[2].IsNextAcvtive = true;
			steps[2].IsActivate = true;
			CurrentStep = 2;
		}

		private void UpdateSelectedCount()
		{
			PassedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "PASS" && x.IsSelected).ToList().Count;
			if (PassedSelectedCount == 0)
			{
				passedSelected = false;
				OnPropertyChanged("PassedSelected");
			}
			else if (PassedSelectedCount < PassedCount)
			{
				passedSelected = null;
				OnPropertyChanged("PassedSelected");
			}
			else if (PassedSelectedCount == PassedCount)
			{
				passedSelected = true;
				OnPropertyChanged("PassedSelected");
			}
			FailedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "FAIL" && x.IsSelected).ToList().Count;
			if (FailedSelectedCount == 0)
			{
				failedSelected = false;
				OnPropertyChanged("FailedSelected");
			}
			else if (FailedSelectedCount < FailedCount)
			{
				failedSelected = null;
				OnPropertyChanged("FailedSelected");
			}
			else if (FailedSelectedCount == FailedCount)
			{
				failedSelected = true;
				OnPropertyChanged("FailedSelected");
			}
			SkippedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "SKIP" && x.IsSelected).ToList().Count;
			if (SkippedSelectedCount == 0)
			{
				skippedSelected = false;
				OnPropertyChanged("SkippedSelected");
			}
			else if (SkippedSelectedCount < SkippedCount)
			{
				skippedSelected = null;
				OnPropertyChanged("SkippedSelected");
			}
			else if (SkippedSelectedCount == SkippedCount)
			{
				skippedSelected = true;
				OnPropertyChanged("SkippedSelected");
			}
		}
		#endregion
	}
}
