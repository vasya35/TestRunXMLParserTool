using DynamicData.Binding;
using Microsoft.Win32;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
			SelectedTestCaseResult = new TestCaseResultModel();
			DisplayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
			PassedSelected = false;
			FailedSelected = false;
			SkippedSelected = false;
			SortSelected = false;
			genXMLCommand = new XMLGeneratorCommand();
			genJQueryScriptCommand = new JSTestrailSelectorScriptGeneratorCommand();

			Steps = new List<StepDescription>() {
				new StepDescription()
				{
					Name = Properties.Resources.OpenFileStep,
					IsActivate = true,
					ActivateAction = ReactiveCommand.Create(Step1Activate),
					IsFirstStep = true,
					IsLastStep = false
				},
				new StepDescription()
				{
					Name = Properties.Resources.FilterAndSortStep,
					IsActivate = false,
					ActivateAction = ReactiveCommand.Create(Step2Activate),
					IsFirstStep = false,
					IsLastStep = false
				},
				new StepDescription()
				{
					Name = Properties.Resources.GenerateFileStep,
					IsActivate = false,
					ActivateAction = ReactiveCommand.Create(Step3Activate),
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
		private readonly MainWindowView mainWindowView;
		private readonly SettingsViewModel settingsViewModel = new();
		#endregion

		#region Properties
		public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; } = new();
		[Reactive] public ObservableCollection<TestCaseResultModel> DisplayedTestCaseResults { get; set; }

		[Reactive] public TestCaseResultModel SelectedTestCaseResult { get; set; }

		[Reactive] public bool? PassedSelected { get; set; }

		[Reactive] public bool? FailedSelected { get; set; }

		[Reactive] public bool? SkippedSelected { get; set; }

		[Reactive] public bool SortSelected { get; set; }

		[Reactive] public string SelectedPath { get; set; } = string.Empty;

		[Reactive] public int PassedCount { get; set; }

		[Reactive] public int FailedCount { get; set; }

		[Reactive] public int SkippedCount { get; set; }

		[Reactive] public int PassedSelectedCount { get; set; }

		[Reactive] public int FailedSelectedCount { get; set; }

		[Reactive] public int SkippedSelectedCount { get; set; }

		[Reactive] public List<StepDescription> Steps { get; set; }

		[Reactive] public int CurrentStep { get; set; }

		#endregion

		#region implementation IReactiveCommand
		private ICommand genXMLCommand;
		private ICommand genJQueryScriptCommand;
		private IReactiveCommand? openXMLCommand;
		private IReactiveCommand? settingsButtonCommand;

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

		public IReactiveCommand OpenXMLCommand
		{
			get
			{
				openXMLCommand ??= ReactiveCommand.Create(ExecuteOpenFileDialog);
				return openXMLCommand;
			}
		}

		public IReactiveCommand SettingsButtonCommand
		{
			get
			{
				settingsButtonCommand ??= ReactiveCommand.Create(OpenSettingsWindow);
				return settingsButtonCommand;
			}
		}
		#endregion

		#region Private methods
		private void ReInitialisation()
		{
			if (SelectedPath == "") return;

			SelectedTestCaseResult = new TestCaseResultModel();
			DisplayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
			PassedSelected = false;
			FailedSelected = false;
			SkippedSelected = false;
			SortSelected = false;
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

			if (SortSelected)
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
			if (mainWindowView.OwnedWindows.Count == 0)
			{
				var settingsWindow = new SettingsWindowView(settingsViewModel)
				{
					Owner = mainWindowView
				};
				settingsWindow.Show();
			}
			else
			{
				mainWindowView.OwnedWindows[0].Activate();
			}
		}

		private void UpdateCounts()
		{
			PassedCount = OriginalTestCaseResults.Where(x => x.Result == "PASS").ToList().Count;
			FailedCount = OriginalTestCaseResults.Where(x => x.Result == "FAIL").ToList().Count;
			SkippedCount = OriginalTestCaseResults.Where(x => x.Result == "SKIP").ToList().Count;
		}

		private void Step1Activate()
		{
			Steps[0].IsActivate = true;
			Steps[0].IsNextAcvtive = false;

			Steps[1].IsActivate = false;
			Steps[1].IsNextAcvtive = false;

			Steps[2].IsActivate = false;
			CurrentStep = 0;
		}

		private void Step2Activate()
		{
			Steps[0].IsActivate = true;
			Steps[0].IsNextAcvtive = true;

			Steps[1].IsActivate = true;
			Steps[1].IsNextAcvtive = false;

			Steps[2].IsActivate = false;
			Steps[2].IsNextAcvtive = false;
			CurrentStep = 1;
		}

		private void Step3Activate()
		{
			Steps[0].IsActivate = true;
			Steps[0].IsNextAcvtive = true;

			Steps[1].IsNextAcvtive = true;
			Steps[1].IsActivate = true;

			Steps[2].IsNextAcvtive = true;
			Steps[2].IsActivate = true;
			CurrentStep = 2;
		}

		private void UpdateSelectedCount()
		{
			PassedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "PASS" && x.IsSelected).ToList().Count;
			if (PassedSelectedCount == 0)
			{
				PassedSelected = false;
			}
			else if (PassedSelectedCount < PassedCount)
			{
				PassedSelected = null;
			}
			else if (PassedSelectedCount == PassedCount)
			{
				PassedSelected = true;
			}
			FailedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "FAIL" && x.IsSelected).ToList().Count;
			if (FailedSelectedCount == 0)
			{
				FailedSelected = false;
			}
			else if (FailedSelectedCount < FailedCount)
			{
				FailedSelected = null;
			}
			else if (FailedSelectedCount == FailedCount)
			{
				FailedSelected = true;
			}
			SkippedSelectedCount = DisplayedTestCaseResults.Where(x => x.Result == "SKIP" && x.IsSelected).ToList().Count;
			if (SkippedSelectedCount == 0)
			{
				SkippedSelected = false;
			}
			else if (SkippedSelectedCount < SkippedCount)
			{
				SkippedSelected = null;
			}
			else if (SkippedSelectedCount == SkippedCount)
			{
				SkippedSelected = true;
			}
		}
		#endregion
	}
}
