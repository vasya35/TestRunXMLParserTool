using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace TestRunXMLParserTool.Models
{
	public class TestCaseResultModel : ReactiveObject
	{
		#region Fields
		private int testCaseNumber;
		#endregion

		#region .ctor
		public TestCaseResultModel()
		{
			this.WhenAnyValue(x => x.Name).Subscribe(x => NameUpdated(Name));
		}
		#endregion

		#region Properties
		/// <summary>
		/// Name
		/// </summary>
		[Reactive] public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Result: PASS, FAIL, SKIP
		/// </summary>
		[Reactive] public string Result { get; set; } = string.Empty;

		/// <summary>
		/// XML path in project
		/// </summary>
		[Reactive] public string XMLPath { get; set; } = string.Empty;

		/// <summary>
		/// Method name
		/// </summary>
		[Reactive] public string MethodName { get; set; } = string.Empty;

		/// <summary>
		/// Selected for operation (generate new xml or generate jquery script)
		/// </summary>
		[Reactive] public bool IsSelected { get; set; } = true;

		/// <summary>
		/// GetTestRailNumber
		/// </summary>
		[Reactive] public string TestRailNumber { get; private set; } = string.Empty;
		
		#endregion

		#region Private methods
		private void NameUpdated(string newName)
		{
			setTestCaseNumber(newName);
			setTestRailNumber(newName);
		}

		private void setTestCaseNumber(string name)
		{
			_ = int.TryParse(name.Split(" ")[0].Trim(new char[] { 'C', 'T' }), out int res);
			testCaseNumber = res;
		}

		private void setTestRailNumber(string value) => TestRailNumber = (value.Split(" ")[0]);
		#endregion

		#region Public methods
		public int getTestCaseNumber()
		{
			return testCaseNumber;
		}
		#endregion

	}
}
