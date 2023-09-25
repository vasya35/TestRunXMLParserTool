using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace TestRunXMLParserTool.Models;

public class JSTestRailSelectorScriptGeneratorModel
{
	#region Fileds
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
	#endregion

	#region Public Methods
	public static async Task<Tuple<bool, string, string>> GenerateAsync(ObservableCollection<TestCaseResultModel> selectedTestCases)
	{
		var genResult = await Task.Run(() => Generate(selectedTestCases));
		return genResult;
	}

	public static Tuple<bool, string, string> Generate(ObservableCollection<TestCaseResultModel> selectedTestCases)
	{
		string filename = "";
		try
		{
			var defaultFileName = "JSTestRailSelector";
			var defaultExtensionFileName = ".js";
			filename = $"{defaultFileName}{defaultExtensionFileName}";

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
				int cnt = 0;
				foreach (var testCase in selectedTestCases)
				{
					if (!testCase.IsSelected) continue;
					testCasesList += $"'{testCase.TestRailNumber}', ";
					cnt++;
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
				writer.WriteLine("var xpath = \"//tr[contains(@class, 'oddSelected') or contains(@class, 'evenSelected')]\"");
				writer.WriteLine("const result = document.evaluate(xpath, document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);");
				writer.WriteLine($"console.log('TestCases are selected: ' + result.snapshotLength + ' from {cnt} expected');");
			}

			return Tuple.Create(true, filename, "");
		}
		catch (Exception e)
		{
			Logger.Error($"Error while generate JS script file: {e}");

			return Tuple.Create(false, filename, e.ToString());
		}

	}
	#endregion

}