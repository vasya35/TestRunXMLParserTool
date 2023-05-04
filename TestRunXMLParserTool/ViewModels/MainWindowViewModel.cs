using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using TestRunXMLParserTool.Commands;
using TestRunXMLParserTool.Models;
using TestRunXMLParserTool.Views;

namespace TestRunXMLParserTool.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
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

			this.WhenAnyValue(x => x.SelectedPath).Subscribe(_ => ReInitialisation());
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
		private readonly MainWindowView mainWindowView;
		private readonly SettingsViewModel settingsViewModel = new();
		#endregion

		#region Properties
		public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; } = new();
		public ObservableCollection<TestCaseResultModel> DisplayedTestCaseResults
		{
			get => displayedTestCaseResults;
			set
			{
				this.RaiseAndSetIfChanged(ref displayedTestCaseResults, value);
			}
		}

		public TestCaseResultModel SelectedTestCaseResult
		{
			get => selectedTestCaseResult;
			set
			{
				this.RaiseAndSetIfChanged(ref selectedTestCaseResult, value);
			}
		}

		public bool? PassedSelected
		{
			get => passedSelected;
			set
			{
				this.RaiseAndSetIfChanged(ref passedSelected, value);
			}
		}

		public bool? FailedSelected
		{
			get => failedSelected;
			set
			{
				this.RaiseAndSetIfChanged(ref failedSelected, value);
			}
		}

		public bool? SkippedSelected
		{
			get => skippedSelected;
			set
			{
				this.RaiseAndSetIfChanged(ref skippedSelected, value);
			}
		}

		public bool SortSelected
		{
			get => sortSelected;
			set
			{
				this.RaiseAndSetIfChanged(ref sortSelected, value);
			}
		}

		public string SelectedPath
		{
			get => selectedPath;
			set
			{
				this.RaiseAndSetIfChanged(ref selectedPath, value);
			}
		}

		public int PassedCount
		{
			get => passedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref passedCount, value);
			}
		}

		public int FailedCount
		{
			get => failedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref failedCount, value);
			}
		}

		public int SkippedCount
		{
			get => skippedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref skippedCount, value);
			}
		}

		public int PassedSelectedCount
		{
			get => passedSelectedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref passedSelectedCount, value);
			}
		}

		public int FailedSelectedCount
		{
			get => failedSelectedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref failedSelectedCount, value);
			}
		}

		public int SkippedSelectedCount
		{
			get => skippedSelectedCount;
			set
			{
				this.RaiseAndSetIfChanged(ref skippedSelectedCount, value);
			}
		}

		public List<StepDescription> Steps
		{
			get => steps;
			set
			{
				this.RaiseAndSetIfChanged(ref steps, value);
			}
		}

		public int CurrentStep
		{
			get => currentStep;
			set
			{
				this.RaiseAndSetIfChanged(ref currentStep, value);
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

			Step2Activate();
			DisplayedTestCaseResults = OriginalTestCaseResults;

			UpdateCounts();

			PassedSelected = true;
			FailedSelected = true;
			SkippedSelected = true;

			this.WhenAnyValue(x => x.PassedSelected,
				x => x.FailedSelected,
				x => x.SkippedSelected,
				x => x.SortSelected).Subscribe(_ => updateFilteredAndSortData());

			foreach (var testCase in OriginalTestCaseResults)
			{
				testCase.WhenAnyPropertyChanged().Subscribe(_ => UpdateSelectedCount());
			}
		}

		private static ObservableCollection<TestCaseResultModel> sortData(ObservableCollection<TestCaseResultModel> filteredData)
		{
			return new ObservableCollection<TestCaseResultModel>(filteredData.OrderBy(x => x.getTestCaseNumber()));
		}

		private void updateFilteredAndSortData()
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

			foreach (var item in filteredData)
			{
				foreach (var status in changingStatus)
				{
					if (item.Result == status)
					{
						if (!item.IsSelected) item.IsSelected = true;
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
			var settingsWindow = new SettingsWindowView(settingsViewModel)
			{
				Owner = mainWindowView
			};
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
				this.RaisePropertyChanged(nameof(PassedSelected));
			}
			else if (PassedSelectedCount < PassedCount)
			{
				passedSelected = null;
				this.RaisePropertyChanged(nameof(PassedSelected));
			}
			else if (PassedSelectedCount == PassedCount)
			{
				passedSelected = true;
				this.RaisePropertyChanged(nameof(PassedSelected));
			}
			FailedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "FAIL" && x.IsSelected).ToList().Count;
			if (FailedSelectedCount == 0)
			{
				failedSelected = false;
				this.RaisePropertyChanged(nameof(FailedSelected));
			}
			else if (FailedSelectedCount < FailedCount)
			{
				failedSelected = null;
				this.RaisePropertyChanged(nameof(FailedSelected));
			}
			else if (FailedSelectedCount == FailedCount)
			{
				failedSelected = true;
				this.RaisePropertyChanged(nameof(FailedSelected));
			}
			SkippedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "SKIP" && x.IsSelected).ToList().Count;
			if (SkippedSelectedCount == 0)
			{
				skippedSelected = false;
				this.RaisePropertyChanged(nameof(SkippedSelected));
			}
			else if (SkippedSelectedCount < SkippedCount)
			{
				skippedSelected = null;
				this.RaisePropertyChanged(nameof(SkippedSelected));
			}
			else if (SkippedSelectedCount == SkippedCount)
			{
				skippedSelected = true;
				this.RaisePropertyChanged(nameof(SkippedSelected));
			}
		}
		#endregion
	}
}
