﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;

namespace TestRunXMLParserTool.Models
{
	public class XMLParserModel
	{
		#region fileds
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		#endregion

		#region Public Methods
		public static async Task<Tuple<bool, string, ObservableCollection<TestCaseResultModel>>> ParseAsync(string path)
		{
			return await Task.Run(() => Parse(path));
		}

		public static Tuple<bool, string, ObservableCollection<TestCaseResultModel>> Parse(string path)
		{
			try
			{
				ObservableCollection<TestCaseResultModel> testCaseResults = new();
				XmlDocument xDoc = new();

				xDoc.Load(path);
				// Get root element
				XmlElement? xRoot = xDoc.DocumentElement;
				if (xRoot != null)
				{
					XmlNodeList tests = xRoot.GetElementsByTagName("test");

					// Testcases traversal
					foreach (XmlNode test in tests)
					{
						if (test == null || test.Attributes == null) continue;

						var testCaseResult = new TestCaseResultModel
						{
							Name = (test.Attributes.GetNamedItem("name") != null) ? test.Attributes.GetNamedItem("name").Value! : ""
						};

						XmlNodeList? testClass = test.SelectNodes("class");
						if (testClass == null || testClass.Count == 0 || testClass[0] == null || testClass[0].Attributes == null) continue;

						testCaseResult.XMLPath = (testClass[0].Attributes.GetNamedItem("name") != null) ? testClass[0].Attributes.GetNamedItem("name").Value! : "";

						XmlNodeList? testMethods = testClass[0].SelectNodes("test-method");

						foreach (XmlNode testMethod in testMethods)
						{
							if (testMethod.Attributes.GetNamedItem("is-config") != null)
							{
								continue;
							}
							testCaseResult.Result = (testMethod.Attributes.GetNamedItem("status") != null) ? testMethod.Attributes.GetNamedItem("status").Value! : "SKIP";
							testCaseResult.MethodName = (testMethod.Attributes.GetNamedItem("name") != null || testMethod.Attributes.GetNamedItem("name").Value != null) ? testMethod.Attributes.GetNamedItem("name").Value! : "";
						}
						testCaseResults.Add(testCaseResult);
					}
				}
				return Tuple.Create(true, "", testCaseResults);
			}
			catch (Exception e)
			{
				Logger.Error($"Error while open XML file: {e}");

				return Tuple.Create(false, e.ToString(), new ObservableCollection<TestCaseResultModel>());
			}

		}
		#endregion
	}
}