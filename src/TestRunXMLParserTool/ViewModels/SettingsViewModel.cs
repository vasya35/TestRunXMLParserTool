using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.ViewModels;

public class SettingsViewModel : ReactiveObject
{
	#region Properties
	public List<string> Languages { get; }
	[Reactive] public string ListenerName { get; set; }

	[Reactive] public string LanguageSelected { get; set; }
	#endregion

	#region .ctor
	public SettingsViewModel()
	{
		Languages = new();
		Languages = AppConfiguration.GetLanguagesList();
		ListenerName = AppConfiguration.GetListenerName();
		LanguageSelected = AppConfiguration.GetLanguage();

		this.WhenAnyValue(x => x.ListenerName).Subscribe(_ => SaveListenerNameToConfig());
		this.WhenAnyValue(x => x.LanguageSelected).Subscribe(_ => SaveLanguageToConfig());
	}
	#endregion

	#region Private Methods
	private void SaveListenerNameToConfig()
	{
		AppConfiguration.SetListenerName(ListenerName);
		var newListenerName = AppConfiguration.GetListenerName();

		if (newListenerName != ListenerName)
		{
			ListenerName = newListenerName;
		}
	}

	private void SaveLanguageToConfig()
	{
		AppConfiguration.SetCulture(LanguageSelected);
		var newLanguageSelected = AppConfiguration.GetLanguage();

		if (newLanguageSelected != LanguageSelected)
		{
			LanguageSelected = newLanguageSelected;
		}
	}
	#endregion

	#region Implementation IReactiveCommand
	private IReactiveCommand? setNewListenerNameCommand;

	public IReactiveCommand SetNewListenerNameCommand
	{
		get
		{
			setNewListenerNameCommand ??= ReactiveCommand.Create(SaveListenerNameToConfig);
			return setNewListenerNameCommand;
		}
	}
	#endregion
}
