using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;

namespace TestRunXMLParserTool.Models
{
	public class XMLGeneratorModel
	{
		#region Pablic Methods
		/// <summary>
		/// Generate xml file with selected test cases
		/// </summary>
		/// <param name="selectedTestCases"></param>
		public static async Task<bool> GenerateAsync(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			return await Task.Run(() => Generate(selectedTestCases));
		}

		public static bool Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
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

				if (result == false) return false;

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
				return true;
			}
			catch (Exception)
			{
				//todo: add to log

				return false;
			}
		}
		#endregion
	}
}
