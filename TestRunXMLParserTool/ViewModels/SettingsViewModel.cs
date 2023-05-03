using ReactiveUI;
using System.Collections.Generic;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.ViewModels
{
	public class SettingsViewModel : ReactiveObject
	{
		#region Fields
		private string listenerName;
		private string languageSelected = "";
		#endregion

		#region Properties
		public List<string> Languages { get; }
		public string ListenerName
		{
			get => listenerName;
			set
			{
				AppConfiguration.SetListenerName(value);
				var newListenerName = AppConfiguration.GetCurrentListenerName();
				this.RaiseAndSetIfChanged(ref listenerName, newListenerName);
			}
		}

		public string LanguageSelected
		{
			get => languageSelected;
			set
			{
				AppConfiguration.SetCulture(value);
				var newLanguageSelected = AppConfiguration.GetCurrentLanguage();
				this.RaiseAndSetIfChanged(ref languageSelected, newLanguageSelected);
			}
		}
		#endregion

		#region .ctor
		public SettingsViewModel()
		{
			Languages = new();
			Languages = AppConfiguration.GetLanguagesList();
			listenerName = AppConfiguration.GetCurrentListenerName();
			LanguageSelected = AppConfiguration.GetCurrentLanguage();
		}
		#endregion

	}
}
