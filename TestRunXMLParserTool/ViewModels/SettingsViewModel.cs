using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.ViewModels
{
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
			ListenerName = AppConfiguration.GetCurrentListenerName();
			LanguageSelected = AppConfiguration.GetCurrentLanguage();

			this.WhenAnyValue(x => x.ListenerName).Subscribe(_ => SetNewListenerName());
			this.WhenAnyValue(x => x.LanguageSelected).Subscribe(_ => SetNewLanguage());
		}
		#endregion

		#region Private Methods
		private void SetNewListenerName()
		{
			AppConfiguration.SetListenerName(ListenerName);
			var newListenerName = AppConfiguration.GetCurrentListenerName();

			if (newListenerName != ListenerName)
			{
				ListenerName = newListenerName;
			}
		}

		private void SetNewLanguage()
		{
			AppConfiguration.SetCulture(LanguageSelected);
			var newLanguageSelected = AppConfiguration.GetCurrentLanguage();

			if (newLanguageSelected != LanguageSelected)
			{
				LanguageSelected = newLanguageSelected;
			}
		}
		#endregion
	}
}
