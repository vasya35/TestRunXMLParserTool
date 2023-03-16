using System.Collections.ObjectModel;
using System.Xml;

namespace TestRunXMLParserTool.Models
{
	public class XMLParserModel
	{
		public static ObservableCollection<TestCaseResultModel> Parse(string path)
		{
			ObservableCollection<TestCaseResultModel> testCaseResults = new();
			XmlDocument xDoc = new();
			//todo: error if file buzy other process
			xDoc.Load(path);
			// Get root element
			XmlElement? xRoot = xDoc.DocumentElement;
			if (xRoot != null)
			{
				XmlNodeList tests = xRoot.GetElementsByTagName("test");

				// Testcases traversal
				foreach (XmlNode test in tests)
				{
					var testCaseResult = new TestCaseResultModel
					{
						Name = test.Attributes.GetNamedItem("name").Value
					};

					XmlNodeList? testClass = test.SelectNodes("class");
					if (testClass.Count < 1) continue;

					testCaseResult.XMLPath = testClass[0].Attributes.GetNamedItem("name").Value;

					XmlNodeList? testMethods = testClass[0].SelectNodes("test-method");

					foreach (XmlNode testMethod in testMethods)
					{
						if (testMethod.Attributes.GetNamedItem("is-config") == null)
						{
							testCaseResult.Result = testMethod.Attributes.GetNamedItem("status")?.Value;
							testCaseResult.MethodName = testMethod.Attributes.GetNamedItem("name")?.Value;
						}
					}

					testCaseResults.Add(testCaseResult);

				}
			}
			return testCaseResults;
		}
	}
}