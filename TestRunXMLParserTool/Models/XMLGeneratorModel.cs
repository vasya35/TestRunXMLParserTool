using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Xml;

namespace TestRunXMLParserTool.Models
{
	public class XMLGeneratorModel
	{
		/// <summary>
		/// Generate xml file with selected test cases
		/// </summary>
		/// <param name="selectedTestCases"></param>
		public static void Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			var defaultFileName = "SelectedTestCases";
			var defaultExtensionFileName = ".xml";
			string filename = $"{defaultFileName}{defaultExtensionFileName}";

			SaveFileDialog saveFileDialog = new()
			{
				FileName = defaultFileName,
				DefaultExt = defaultExtensionFileName,
				Filter = "Text documents (.xml)|*.xml"
			};

			bool? result = saveFileDialog.ShowDialog();

			if (result == true)
			{
				filename = saveFileDialog.FileName;
			}

			XmlWriterSettings xmlWriterSettings = new()
			{
				Indent = true,
				NewLineOnAttributes = false
			};

			using XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings);
			writer.WriteStartDocument();
			writer.WriteStartElement("suite");
			writer.WriteAttributeString("name", "rerunTestCases");
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
		}
	}
}
