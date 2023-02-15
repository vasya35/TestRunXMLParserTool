using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace TestRunXMLParserTool.Models
{
	internal class JSTestRailSelectorScriptGeneratorModel
	{
		public JSTestRailSelectorScriptGeneratorModel()
		{
		}

		internal void Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
		{
			var defaultFileName = "JSTestRailSelector";
			var defaultExtensionFileName = ".js";
			string filename = $"{defaultFileName}{defaultExtensionFileName}";

			SaveFileDialog saveFileDialog = new()
			{
				FileName = defaultFileName,
				DefaultExt = defaultExtensionFileName,
				Filter = "Text documents (.js)|*.js"
			};

			bool? result = saveFileDialog.ShowDialog();

			if (result == true)
			{
				filename = saveFileDialog.FileName;
			}
			using (TextWriter writer = TextWriter.Synchronized(new StreamWriter(filename)))
			{
				string testCasesList = "";
				foreach (var testCase in selectedTestCases)
				{
					if (!testCase.IsSelected) continue;
					testCasesList += $"'{testCase.TestRailNumber}', ";
				}
				if (testCasesList.Length > 2)
				{
					testCasesList = testCasesList.Remove(testCasesList.Length - 2, 2);
				}

				writer.WriteLine($"const testCasesArray = [{testCasesList}];");
				writer.WriteLine("var xPathRes;");
				writer.WriteLine("testCasesArray.forEach(element => {");
				writer.WriteLine("xPathRes = document.evaluate (\"//a[text()='\"+element+\"']/../../td[@class='checkbox']/input\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);");
				writer.WriteLine("xPathRes.singleNodeValue.click();");
				writer.WriteLine("});");
			}

		}
	}
}