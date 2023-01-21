using System;
using System.Collections.ObjectModel;
using System.Xml;
using TestRunXMLParserTool;

namespace TestRunXMLParserTool
{
	public class XMLParser
	{
		public ObservableCollection<TestCaseResultModel> Parse(String path)
		{
            ObservableCollection<TestCaseResultModel> testCaseResults = new ObservableCollection<TestCaseResultModel> ();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                XmlNodeList tests = xRoot.GetElementsByTagName("test");

				// обход всех тестов
				foreach (XmlNode test in tests)
				{
                    var testCaseResult = new TestCaseResultModel();
                    testCaseResult.Name = test.Attributes.GetNamedItem("name").Value;

                    // обходим все дочерние узлы элемента user
                    XmlNodeList? testMethods = test.SelectNodes("class/test-method");

					foreach (XmlNode testMethod in testMethods)
					{
						if (testMethod.Attributes.GetNamedItem("is-config") == null)
						{
                            testCaseResult.Result = testMethod.Attributes.GetNamedItem("status")?.Value;
                        }
						
					}

                    testCaseResults.Add(testCaseResult);
                }
            }
            return testCaseResults;
        }
	}
}