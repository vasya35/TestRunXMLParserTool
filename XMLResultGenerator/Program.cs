using XMLResultGenerator;

internal class Program
{
	private static void Main()
	{
		Console.WriteLine("Enter count for generaate test case results:");
		var inputCnt = Console.ReadLine();

		if (int.TryParse(inputCnt, out int cnt))
		{
			TestrunResultsXMLGenerator.Generate(cnt);
		}
	}
}