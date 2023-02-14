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
		public void Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			var defaultFileName = "SelectedTestCases";
			var defaultExtensionFileName = ".xml";
			string filename = $"{defaultFileName}{defaultExtensionFileName}";

			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.FileName = defaultFileName;
			saveFileDialog.DefaultExt = defaultExtensionFileName;
			saveFileDialog.Filter = "Text documents (.xml)|*.xml";

			bool? result = saveFileDialog.ShowDialog();

			if (result == true)
			{
				filename = saveFileDialog.FileName;
			}

			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
			{
				Indent = true,
				NewLineOnAttributes = false
			};

			using (XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("suite");
				writer.WriteAttributeString("name", "rerunTestCases");

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
}
