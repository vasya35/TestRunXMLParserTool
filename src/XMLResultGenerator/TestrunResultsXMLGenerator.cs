using System.Xml;

namespace XMLResultGenerator;

static internal class TestrunResultsXMLGenerator
{
	class TestCaseGenerateResults
	{
		public int number;
		public TestCaseStatus status;
	}
	enum TestCaseStatus
	{
		FAIL,
		PASS,
		SKIP
	}

	internal static void Generate(int cnt)
	{

		var defaultFileName = "GeneratedTestCases";
		var defaultExtensionFileName = ".xml";
		string filename = $"{defaultFileName}{defaultExtensionFileName}";


		XmlWriterSettings xmlWriterSettings = new()
		{
			Indent = true,
			NewLineOnAttributes = false
		};

		HashSet<int> testCaseIDs = new() { };

		List<TestCaseGenerateResults> testCases = new();
		var values = Enum.GetValues(typeof(TestCaseStatus)).Cast<TestCaseStatus>().ToList(); ;
		Random randomStatus = new();
		Random randomID = new();

		for (int i = 0; i < cnt; i++)
		{
			int id;
			do
			{
				id = randomID.Next(cnt * 1000);
			} while (testCaseIDs.Add(id));
			var newTestCaseResult = new TestCaseGenerateResults
			{
				number = id,
				status = values[randomStatus.Next(values.Count)]
			};

			testCases.Add(newTestCaseResult);

		};

		using XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings);
		writer.WriteStartDocument();
		writer.WriteStartElement("testng-results");
		writer.WriteAttributeString("skipped", testCases.Select(x => x.status == TestCaseStatus.SKIP).ToList().Count.ToString());
		writer.WriteAttributeString("failed", testCases.Select(x => x.status == TestCaseStatus.FAIL).ToList().Count.ToString());
		writer.WriteAttributeString("passed", testCases.Select(x => x.status == TestCaseStatus.PASS).ToList().Count.ToString());
		writer.WriteAttributeString("total", testCases.Count.ToString());

		// generate results:
		foreach (var testCase in testCases)
		{
			writer.WriteStartElement("test");
			writer.WriteAttributeString("name", $"C{testCase.number} - TestCaseNumber_{testCase.number}");
			writer.WriteStartElement("class");
			writer.WriteAttributeString("name", $"com.test.case.GenerateTestCaseNumber{testCase.number}");
			writer.WriteStartElement("test-method");


			writer.WriteAttributeString("status", testCase.status.ToString());
			writer.WriteAttributeString("name", $"testCaseGenerator_{testCase.number}");

			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Flush();


	}
}
