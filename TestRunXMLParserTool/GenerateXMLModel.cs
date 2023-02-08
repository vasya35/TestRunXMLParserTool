using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Xml;

namespace TestRunXMLParserTool
{
	public class GenerateXMLModel
	{
		public void Generate(ObservableCollection<TestCaseResultModel>? selectedTestCases)
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
					writer.WriteStartElement("test");
					writer.WriteAttributeString("name", testCase.Name);
					writer.WriteStartElement("classes");
					writer.WriteStartElement("class");
					//TODO: add path for test case
					writer.WriteAttributeString("name", "PATH");
					writer.WriteStartElement("methods");
					writer.WriteStartElement("include");
					// TODO: add testcase name
					writer.WriteAttributeString("name", "testSaleSingleEvent");
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
