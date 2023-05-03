using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.ViewModels
{
	internal class SettingsViewModel : INotifyPropertyChanged
	{
		#region Fields
		private string listenerName;
		private string languageSelected = "";
		#endregion

		#region Properties
		public List<string> Languages { get; }
		public string ListenerName {
			get
			{
				return listenerName;
			} 
			set 
			{
				if (value != listenerName)
				{
					listenerName = value;
					AppConfiguration.SetListenerName(value);
					listenerName = AppConfiguration.GetCurrentListenerName();
					OnPropertyChanged("ListenerName");
				}				
			} 
		}

		public string LanguageSelected
		{
			get => languageSelected;
			set
			{
				if (value != languageSelected)
				{
					languageSelected = value;
					AppConfiguration.SetCulture(languageSelected);
					languageSelected = AppConfiguration.GetCurrentLanguage();
					OnPropertyChanged("LanguageSelected");
				}
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

		#region Implementation INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion

	}
}
