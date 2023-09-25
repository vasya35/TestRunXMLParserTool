using TestRunXMLParserTool.Models;

namespace TestRunXMLParserToolUnitTests;

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void Test1OnePass()
	{
		string path = "TestRunXMLParserFiles\\Test1.xml";
		var parseResult = XMLParserModel.Parse(path);
		Assert.That(parseResult.Item3.Count(), Is.EqualTo(1), "TestCaseResult count isn't correct");
		TestCaseResultModel testCaseResult = parseResult.Item3[0];
		Assert.That(testCaseResult.Result, Is.EqualTo("PASS"), "Status wasn't parsed correct");
		Assert.That(testCaseResult.Name, Is.EqualTo("C1 - TestCaseNumber_1"), "Test case name wasn't parsed correct");
		Assert.That(testCaseResult.XMLPath, Is.EqualTo("com.test.case.GenerateTestCaseNumber1"), "Test case XML path wasn't parsed correct");
		Assert.That(testCaseResult.MethodName, Is.EqualTo("testCaseGenerator_1"), "Test case method name wasn't parsed correct");
	}

	[Test]
	public void Test2OneFail()
	{
		string path = "TestRunXMLParserFiles\\Test2.xml";
		var parseResult = XMLParserModel.Parse(path);
		Assert.That(parseResult.Item3.Count(), Is.EqualTo(1), "TestCaseResult count isn't correct");
		TestCaseResultModel testCaseResult = parseResult.Item3[0];
		Assert.That(testCaseResult.Result, Is.EqualTo("FAIL"), "Status wasn't parsed correct");
		Assert.That(testCaseResult.Name, Is.EqualTo("C2 - TestCaseNumber_2"), "Test case name wasn't parsed correct");
		Assert.That(testCaseResult.XMLPath, Is.EqualTo("com.test.case.GenerateTestCaseNumber2"), "Test case XML path wasn't parsed correct");
		Assert.That(testCaseResult.MethodName, Is.EqualTo("testCaseGenerator_2"), "Test case method name wasn't parsed correct");
	}

	[Test]
	public void Test3OneSkip()
	{
		string path = "TestRunXMLParserFiles\\Test3.xml";
		var parseResult = XMLParserModel.Parse(path);
		Assert.That(parseResult.Item3.Count(), Is.EqualTo(1), "TestCaseResult count isn't correct");
		TestCaseResultModel testCaseResult = parseResult.Item3[0];
		Assert.That(testCaseResult.Result, Is.EqualTo("SKIP"), "Status isn't correct", "Status wasn't parsed correct");
		Assert.That(testCaseResult.Name, Is.EqualTo("C3 - TestCaseNumber_3"), "Test case name wasn't parsed correct");
		Assert.That(testCaseResult.XMLPath, Is.EqualTo("com.test.case.GenerateTestCaseNumber3"), "Test case XML path wasn't parsed correct");
		Assert.That(testCaseResult.MethodName, Is.EqualTo("testCaseGenerator_3"), "Test case method name wasn't parsed correct");
	}

	[Test]
	public void Test4OneFailUnusual()
	{
		string path = "TestRunXMLParserFiles\\Test4.xml";
		var parseResult = XMLParserModel.Parse(path);
		Assert.That(parseResult.Item3.Count(), Is.EqualTo(1), "TestCaseResult count isn't correct");
		TestCaseResultModel testCaseResult = parseResult.Item3[0];
		Assert.That(testCaseResult.Result, Is.EqualTo("FAIL"), "Status isn't correct", "Status wasn't parsed correct");
		Assert.That(testCaseResult.Name, Is.EqualTo("C4 - TestCaseNumber_4"), "Test case name wasn't parsed correct");
		Assert.That(testCaseResult.XMLPath, Is.EqualTo("com.test.case.GenerateTestCaseNumber4"), "Test case XML path wasn't parsed correct");
		Assert.That(testCaseResult.MethodName, Is.EqualTo("testCaseGenerator_4"), "Test case method name wasn't parsed correct");
	}

	[Test]
	public void Test5OneFailUnusual()
	{
		string path = "TestRunXMLParserFiles\\Test5.xml";
		var parseResult = XMLParserModel.Parse(path);
		Assert.That(parseResult.Item3.Count(), Is.EqualTo(1), "TestCaseResult count isn't correct");
		TestCaseResultModel testCaseResult = parseResult.Item3[0];
		Assert.That(testCaseResult.Result, Is.EqualTo("FAIL"), "Status isn't correct", "Status wasn't parsed correct");
		Assert.That(testCaseResult.Name, Is.EqualTo("C5 - TestCaseNumber_5"), "Test case name wasn't parsed correct");
		Assert.That(testCaseResult.XMLPath, Is.EqualTo("com.test.case.GenerateTestCaseNumber5"), "Test case XML path wasn't parsed correct");
		Assert.That(testCaseResult.MethodName, Is.EqualTo("testCaseGenerator_5"), "Test case method name wasn't parsed correct");
	}
}