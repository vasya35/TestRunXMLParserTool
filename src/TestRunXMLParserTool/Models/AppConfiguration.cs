using System.Collections.Generic;
using System.Configuration;

namespace TestRunXMLParserTool.Models
{
	static internal class AppConfiguration
	{
		#region fields
		private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly List<string> languages = new() { "English", "Russian", "Kazakh", "Turkish" };

		private static readonly string ListenerNameDefault = "LISTENER_NAME_KEY";
		private static readonly string LanguageDefault = "English";
		private static readonly string CultureDefault = "en-US";
		private static readonly bool SortEnabledDefault = true;
		private static readonly bool ShowOnlySelectedEnabledDefault = false;
		#endregion

		#region event
		public delegate void Notify();
		public static event Notify? CultereChangedEvent;
		#endregion

		#region Public methods

		#region Culture
		public static List<string> GetLanguagesList()
		{
			return languages;
		}

		public static string GetCulture()
		{
			var currentCulture = ConfigurationManager.AppSettings["Culture"];
			if (currentCulture != null && currentCulture != "") return currentCulture;
			SetCulture(LanguageDefault);
			return CultureDefault;
		}


		public static string GetLanguage()
		{
			string culture = GetCulture();
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

			try
			{
				config.AppSettings.Settings["Culture"].Value = culture;
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.NullReferenceException e)
			{
				Logger.Warn($"Culture setting is absent in appconfig: {e}");
				config.AppSettings.Settings.Add("Culture", culture);
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.Exception e)
			{
				Logger.Error($"Error save Culture setting in app.config: {e}");
			}

			CultereChangedEvent?.Invoke();
		}
		#endregion

		#region Listener name
		public static string GetListenerName()
		{
			var currentListenerName = ConfigurationManager.AppSettings["ListenerName"];
			if (currentListenerName != null && currentListenerName != "") return currentListenerName;
			SetListenerName(ListenerNameDefault);
			return ListenerNameDefault;
		}

		public static void SetListenerName(string listenerName)
		{
			try
			{
				config.AppSettings.Settings["listenerName"].Value = listenerName;
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.NullReferenceException e)
			{
				Logger.Warn($"ListenerName setting is absent in appconfig: {e}");
				config.AppSettings.Settings.Add("listenerName", listenerName);
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.Exception e)
			{
				Logger.Error($"Error save listenerName setting in app.config: {e}");
			}
		}
		#endregion

		#region Sort Enabled
		public static bool GetSortEnabled()
		{
			if (bool.TryParse(ConfigurationManager.AppSettings["SortEnabled"], out bool currentSortEnabled))
			{
				return currentSortEnabled;
			}
			SetSortEnabled(SortEnabledDefault);
			return SortEnabledDefault;
		}

		public static void SetSortEnabled(bool sortEnabled)
		{
			try
			{
				config.AppSettings.Settings["SortEnabled"].Value = sortEnabled.ToString();
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.NullReferenceException e)
			{
				Logger.Warn($"SortEnabled setting is absent in appconfig: {e}");
				config.AppSettings.Settings.Add("SortEnabled", sortEnabled.ToString());
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.Exception e)
			{
				Logger.Error($"Error save SortEnabled setting in app.config: {e}");
			}

		}
		#endregion

		#region Show Only Selected Enabled
		public static bool GetShowOnlySelectedEnabled()
		{
			if (bool.TryParse(ConfigurationManager.AppSettings["ShowOnlySelectedEnabled"], out bool currentSortEnabled))
			{
				return currentSortEnabled;
			}
			SetShowOnlySelectedEnabled(ShowOnlySelectedEnabledDefault);
			return ShowOnlySelectedEnabledDefault;
		}

		public static void SetShowOnlySelectedEnabled(bool showOnlySelectedEnabled)
		{
			try
			{
				config.AppSettings.Settings["ShowOnlySelectedEnabled"].Value = showOnlySelectedEnabled.ToString();
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.NullReferenceException e)
			{
				Logger.Warn($"ShowOnlySelectedEnabled setting is absent in appconfig: {e}");
				config.AppSettings.Settings.Add("ShowOnlySelectedEnabled", showOnlySelectedEnabled.ToString());
				config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
			catch (System.Exception e)
			{
				Logger.Error($"Error save ShowOnlySelectedEnabled setting in app.config: {e}");
			}

		}
		#endregion


		#endregion

	}
}
