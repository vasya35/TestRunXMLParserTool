using DynamicData;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestRunXMLParserTool.Models;
using TestRunXMLParserTool.Services;
using TestRunXMLParserTool.Views;

#if DEBUG
using System.Diagnostics;
#endif

namespace TestRunXMLParserTool.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
	#region .ctor
	public MainWindowViewModel(IAbstractFactory<SettingsWindowView> factory)
	{
		this.factory = factory;
		SelectedTestCaseResult = new TestCaseResultModel();
		DisplayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
		PassedSelected = false;
		FailedSelected = false;
		SkippedSelected = false;
		SortSelected = AppConfiguration.GetSortEnabled();
		ShowOnlySelected = AppConfiguration.GetShowOnlySelectedEnabled();

		this.WhenAnyValue(x => x.SortSelected).Subscribe(_ => SetSortSelectedToConfig());
		this.WhenAnyValue(x => x.ShowOnlySelected).Subscribe(_ => SetShowOnlySelectedToConfig());

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

		//ExecuteOpenFileDialog();
	}
	#endregion

	#region Fields
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	private List<IDisposable> isSelectedSubscriptions = new();
	private IAbstractFactory<SettingsWindowView> factory;
	#endregion

	#region Debug
#if DEBUG
	int UpdateCountsHidingAndSortingCount = 0;
	int UpdateFilteringAndSortDataCount = 0;
#endif
	#endregion

	#region Properties
	public ObservableCollection<TestCaseResultModel> OriginalTestCaseResults { get; set; } = new();
	[Reactive] public ObservableCollection<TestCaseResultModel> DisplayedTestCaseResults { get; set; }

	[Reactive] public TestCaseResultModel SelectedTestCaseResult { get; set; }

	[Reactive] public bool? PassedSelected { get; set; }

	[Reactive] public bool? FailedSelected { get; set; }

	[Reactive] public bool? SkippedSelected { get; set; }

	[Reactive] public bool SortSelected { get; set; }

	[Reactive] public bool ShowOnlySelected { get; set; }

	[Reactive] public string SelectedPath { get; set; } = string.Empty;

	[Reactive] public int PassedCount { get; set; }

	[Reactive] public int FailedCount { get; set; }

	[Reactive] public int SkippedCount { get; set; }

	[Reactive] public int PassedSelectedCount { get; set; }

	[Reactive] public int FailedSelectedCount { get; set; }

	[Reactive] public int SkippedSelectedCount { get; set; }

	[Reactive] public List<StepDescription> Steps { get; set; }

	[Reactive] public int CurrentStep { get; set; }
	[Reactive] public bool OpenXMLButtonIsEnabled { get; set; } = true;
	[Reactive] public bool SelectByFolderButtonIsEnabled { get; set; } = true;
	[Reactive] public bool GenXMLButtonIsEnabled { get; set; } = true;
	[Reactive] public bool GenJQueryScriptButtonIsEnabled { get; set; } = true;

	#endregion

	#region Implementation IReactiveCommand
	private IReactiveCommand? genXMLCommand;
	private IReactiveCommand? genJQueryScriptCommand;
	private IReactiveCommand? openXMLCommand;
	private IReactiveCommand? selectByFolderCommand;
	private IReactiveCommand? settingsButtonCommand;

	public IReactiveCommand GenXMLCommand
	{
		get
		{
			genXMLCommand ??= ReactiveCommand.Create<ObservableCollection<TestCaseResultModel>>(x => GenXMLButtonCommand(x));
			return genXMLCommand;
		}
	}

	public IReactiveCommand GenJQueryScriptCommand
	{
		get
		{
			genJQueryScriptCommand ??= ReactiveCommand.Create<ObservableCollection<TestCaseResultModel>>(x => GenJQueryScriptButtonCommand(x));
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

	public IReactiveCommand SelectByFolderCommand
	{
		get
		{
			selectByFolderCommand ??= ReactiveCommand.Create(SelectByFolder);
			return selectByFolderCommand;
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
	private async Task ReInitialisationAsync()
	{
		if (SelectedPath == "") return;

		SelectedTestCaseResult = new TestCaseResultModel();
		DisplayedTestCaseResults = new ObservableCollection<TestCaseResultModel>();
		PassedSelected = false;
		FailedSelected = false;
		SkippedSelected = false;
		SortSelected = AppConfiguration.GetSortEnabled();
		ShowOnlySelected = AppConfiguration.GetShowOnlySelectedEnabled();

		var result = await XMLParserModel.ParseAsync(SelectedPath);

		if (result.Item1 == false && result.Item2 != "")
		{
			string messageBoxText = Properties.Resources.WindowGenOpenFileErrorText;
			string caption = Properties.Resources.WindowGenOpenFileCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Error;
			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}

		if (result.Item1 == true && result.Item2 == "")
		{
			OriginalTestCaseResults = result.Item3;

			Step2Activate();
			DisplayedTestCaseResults = OriginalTestCaseResults;

			UpdateCounts();

			PassedSelected = true;
			FailedSelected = true;
			SkippedSelected = true;

			this.WhenAnyValue(x => x.PassedSelected,
				x => x.FailedSelected,
				x => x.SkippedSelected,
				x => x.SortSelected,
				x => x.ShowOnlySelected).Subscribe(_ => UpdateFilteringAndSortData());

			SubscribeOnTestCaseResultsSelected();
		}
	}

	private void SubscribeOnTestCaseResultsSelected()
	{
		foreach (var testCase in OriginalTestCaseResults)
		{
			isSelectedSubscriptions.Add(testCase.ObservableForProperty(r => r.IsSelected).Subscribe(_ => UpdateCountsHidingAndSorting()));
		}
	}

	private void UnsubscribeOnTestCaseResultsSelected()
	{
		foreach (var subscription in isSelectedSubscriptions)
		{
			subscription.Dispose();
		}
		isSelectedSubscriptions.Clear();
	}


	private static ObservableCollection<TestCaseResultModel> sortData(ObservableCollection<TestCaseResultModel> filteredData)
	{
		return new ObservableCollection<TestCaseResultModel>(filteredData.OrderBy(x => x.getTestCaseNumber()));
	}

	private void UpdateFilteringAndSortData()
	{
#if DEBUG
		UpdateFilteringAndSortDataCount++;
		Debug.WriteLine($"UpdateFilteringAndSortData  method called: {UpdateFilteringAndSortDataCount}");
#endif

		UnsubscribeOnTestCaseResultsSelected();

		List<string> selectedStatus = new();
		List<string> unselectedStatus = new();

		if (PassedSelected == true)
		{
			selectedStatus.Add(new string("PASS"));
		}
		else if (PassedSelected == false)
		{
			unselectedStatus.Add(new string("PASS"));
		}

		if (FailedSelected == true)
		{
			selectedStatus.Add(new string("FAIL"));
		}
		else if (FailedSelected == false)
		{
			unselectedStatus.Add(new string("FAIL"));
		}

		if (SkippedSelected == true)
		{
			selectedStatus.Add(new string("SKIP"));
		}
		else if (SkippedSelected == false)
		{
			unselectedStatus.Add(new string("SKIP"));
		}

		foreach (var item in OriginalTestCaseResults)
		{
			foreach (var status in selectedStatus)
			{
				if (item.Result == status)
				{
					if (!item.IsSelected) item.IsSelected = true;
				}
			}

			foreach (var status in unselectedStatus)
			{
				if (item.Result == status)
				{
					if (item.IsSelected) item.IsSelected = false;
				}
			}
		}

		UpdateCountsHidingAndSorting();

		SubscribeOnTestCaseResultsSelected();
	}

	private void UpdateCountsHidingAndSorting()
	{
#if DEBUG
		UpdateCountsHidingAndSortingCount++;
		Debug.WriteLine($"UpdateCountsHidingAndSorting method called: {UpdateCountsHidingAndSortingCount}");
#endif

		UpdateSelectedCount();
		HidingAndSorting();
	}

	private void HidingAndSorting()
	{
		ObservableCollection<TestCaseResultModel> filteredData = new();
		if (OriginalTestCaseResults != null)
		{
			if (!ShowOnlySelected)
			{
				filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.ToList());
			}
			else
			{
				filteredData = new ObservableCollection<TestCaseResultModel>((IEnumerable<TestCaseResultModel>)OriginalTestCaseResults.Where(x => x.IsSelected == true).ToList());
			}
		}

		if (SortSelected)
		{
			DisplayedTestCaseResults = sortData(filteredData);
		}
		else
		{
			DisplayedTestCaseResults = filteredData;
		}
	}

	private async void ExecuteOpenFileDialog()
	{
		OpenXMLButtonIsEnabled = false;

		var dialog = new OpenFileDialog { };

		if (dialog.ShowDialog() == true)
		{
			SelectedPath = dialog.FileName;
			await ReInitialisationAsync();
		}

		OpenXMLButtonIsEnabled = true;
	}

	private async void SelectByFolder()
	{
		SelectByFolderButtonIsEnabled = false;

		var dialog = new CommonOpenFileDialog();
		dialog.IsFolderPicker = true;
		CommonFileDialogResult result = dialog.ShowDialog();


		if (result == CommonFileDialogResult.Ok)
		{
			SelectedPath = dialog.FileName;
			var testCasesForSelect = await ScreenshotsFolderParsingModel.GetTestCasesByFolderAsync(SelectedPath);

			if (testCasesForSelect.Item3.Count == 0)
			{
				return;
			}

			foreach (var item3 in OriginalTestCaseResults)
			{
				item3.IsSelected = false;
			}

			ObservableCollection<TestCaseResultModel> filteredData = new();

			foreach (var item in testCasesForSelect.Item3)
			{
				var list = OriginalTestCaseResults.Where(x => x.getTestCaseNumber().ToString() == item).ToList();

				filteredData.Add(list);
			}

			DisplayedTestCaseResults = new ObservableCollection<TestCaseResultModel> { };

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
				if (!item.IsSelected) item.IsSelected = true;
			}

			UpdateSelectedCount();
		}
		var test = OriginalTestCaseResults.Where(x => x.IsSelected == true).ToList();

		SelectByFolderButtonIsEnabled = true;
	}

	private void OpenSettingsWindow()
	{
		factory.Create().Show();
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
		PassedSelectedCount = OriginalTestCaseResults.Where(x => x.Result == "PASS" && x.IsSelected).ToList().Count;
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
		FailedSelectedCount = OriginalTestCaseResults.Where(x => x.Result == "FAIL" && x.IsSelected).ToList().Count;
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
		SkippedSelectedCount = OriginalTestCaseResults.Where(x => x.Result == "SKIP" && x.IsSelected).ToList().Count;
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


	private async void GenXMLButtonCommand(ObservableCollection<TestCaseResultModel> testCaseResultModelCollection)
	{
		GenXMLButtonIsEnabled = false;
		var result = await XMLGeneratorModel.GenerateAsync(testCaseResultModelCollection);

		if (result.Item1 == false && result.Item2 != "")
		{
			string messageBoxText = Properties.Resources.WindowGenXMLErrorText;
			string caption = Properties.Resources.WindowGenXMLCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Error;
			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}
		else if (result.Item1 == true)
		{
			string messageBoxText = Properties.Resources.WindowGenXMLSuccessText;
			string caption = Properties.Resources.WindowGenXMLCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Information;
			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}

		GenXMLButtonIsEnabled = true;
	}

	private async void GenJQueryScriptButtonCommand(ObservableCollection<TestCaseResultModel> testCaseResultModelCollection)
	{
		GenJQueryScriptButtonIsEnabled = false;
		var result = await JSTestRailSelectorScriptGeneratorModel.GenerateAsync(testCaseResultModelCollection);

		if (result.Item1 == false && result.Item3 != "")
		{
			string messageBoxText = Properties.Resources.WindowGenJSScriptErrorText;
			string caption = Properties.Resources.WindowGenJSScriptCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Error;
			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}
		else if (result.Item1 == true)
		{
			string messageBoxText = Properties.Resources.WindowGenJSScriptSuccessText;
			string caption = Properties.Resources.WindowGenJSScriptCaption;
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Information;

			if (CopyToClipboard(result.Item2))
			{
				messageBoxText += Properties.Resources.WindowGenJSScriptCopyToClipboardSuccessText;
			}

			_ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
		}

		GenJQueryScriptButtonIsEnabled = true;
	}

	private static bool CopyToClipboard(string path)
	{
		try
		{
			TextReader reader = new StreamReader(path);
			var text = reader.ReadToEnd();
			Clipboard.SetText(text);
			return true;
		}
		catch (Exception e)
		{
			Logger.Error($"Error while copy to clipboard: {e}");

			return false;
		}

	}

	private void SetSortSelectedToConfig()
	{
		AppConfiguration.SetSortEnabled(SortSelected);
	}

	private void SetShowOnlySelectedToConfig()
	{
		AppConfiguration.SetShowOnlySelectedEnabled(ShowOnlySelected);
	}

	#endregion
}
