using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;

namespace TestRunXMLParserTool.Models
{
	public class XMLGeneratorModel
	{
		#region fileds
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		#endregion

		#region Pablic Methods
		/// <summary>
		/// Generate xml file with selected test cases
		/// </summary>
		/// <param name="selectedTestCases"></param>
		public static async Task<Tuple<bool, string>> GenerateAsync(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			return await Task.Run(() => Generate(selectedTestCases));
		}

		public static Tuple<bool, string> Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			try
			{
				var defaultFileName = "SelectedTestCases";
				var defaultExtensionFileName = ".xml";
				string fileName = $"{defaultFileName}{defaultExtensionFileName}";

				SaveFileDialog saveFileDialog = new()
				{
					FileName = defaultFileName,
					DefaultExt = defaultExtensionFileName,
					Filter = "Text documents (.xml)|*.xml"
				};

				bool? result = saveFileDialog.ShowDialog();

				if (result == false) return new Tuple<bool, string>(false, "");

				fileName = saveFileDialog.FileName;

				XmlWriterSettings xmlWriterSettings = new()
				{
					Indent = true,
					NewLineOnAttributes = false
				};

				using XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings);
				writer.WriteStartDocument();
				writer.WriteStartElement("suite");
				writer.WriteAttributeString("name", "selectedTestCases");
				writer.WriteStartElement("listeners");
				writer.WriteStartElement("listener");
				var listenerName = AppConfiguration.GetCurrentListenerName();
				writer.WriteAttributeString("class-name", listenerName);
				writer.WriteEndElement();
				writer.WriteEndElement();


				foreach (var testCase in selectedTestCases)
				{
					if (!testCase.IsSelected) continue;

					writer.WriteStartElement("test");
					writer.WriteAttributeString("name", testCase.Name);
					writer.WriteStartElement("classes");
					writer.WriteStartElement("class");
					writer.WriteAttributeString("name", testCase.XMLPath);
					writer.WriteStartElement("methods");
					writer.WriteStartElement("include");
					writer.WriteAttributeString("name", testCase.MethodName);
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();
				return new Tuple<bool, string>(true, "");
			}
			catch (Exception e)
			{
				Logger.Error($"Error while generate XML file: {e}");

				return new Tuple<bool, string>(false, e.ToString());
			}
		}
		#endregion
	}
}
