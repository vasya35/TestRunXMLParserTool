using System.Collections.Generic;
using System.Configuration;

namespace TestRunXMLParserTool.Models
{
	static internal class AppConfiguration
	{
		#region fields
		private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		private static readonly List<string> languages = new() { "English", "Russian", "Kazakh", "Turkish" };
		#endregion

		#region Public methods
		public static List<string> GetLanguagesList()
		{
			return languages;
		}

		public static string GetCurrentCulture()
		{
			return ConfigurationManager.AppSettings["Culture"];
		}

		public static string GetCurrentLanguage()
		{
			string culture = GetCurrentCulture();
			string language = culture switch
			{
				"en-US" => "English",
				"ru-RU" => "Russian",
				"kk-KZ" => "Kazakh",
				"tr-TR" => "Turkish",
				_ => "",
			};
			return language;
		}

		public static void SetCulture(string language)
		{
			string culture = "";
			switch (language)
			{
				case "English":
					culture = "en-US";
					break;
				case "Russian":
					culture = "ru-RU";
					break;
				case "Kazakh":
					culture = "kk-KZ";
					break;
				case "Turkish":
					culture = "tr-TR";
					break;
				default:
					break;
			}
			config.AppSettings.Settings["Culture"].Value = culture;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");

			CultereChangedEvent?.Invoke();
		}

		public static string GetCurrentListenerName()
		{
			return ConfigurationManager.AppSettings["ListenerName"];
		}

		public static void SetListenerName(string listenerName)
		{
			config.AppSettings.Settings["listenerName"].Value = listenerName;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
		#endregion

		#region event
		public delegate void Notify();
		public static event Notify? CultereChangedEvent;
		#endregion
	}
}
