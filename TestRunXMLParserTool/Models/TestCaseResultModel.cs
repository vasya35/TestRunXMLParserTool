using ReactiveUI;

namespace TestRunXMLParserTool.Models
{
	public class TestCaseResultModel : ReactiveObject
	{
		#region Fields

		private string name;
		private int testCaseNumber;
		private string testRailNumber;
		private string result;
		private string xmlPath;
		private string methodName;
		private bool isSelected;

		#endregion

		#region .ctor
		public TestCaseResultModel()
		{
			name = "";
			result = "";
			xmlPath = "";
			methodName = "";
			testRailNumber = "";
		}
		#endregion

		#region Properties
		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get => name;
			set
			{
				if (value == null) return;
				setTestCaseNumber(value);
				setTestRailNumber(value);
				this.RaiseAndSetIfChanged(ref name, value);
			}
		}

		/// <summary>
		/// Result: PASS, FAIL, SKIP
		/// </summary>
		public string Result
		{
			get { return result; }
			set
			{
				if (value == null) return;
				this.RaiseAndSetIfChanged(ref result, value);
			}
		}

		/// <summary>
		/// XML path in project
		/// </summary>
		public string XMLPath
		{
			get { return xmlPath; }
			set
			{
				this.RaiseAndSetIfChanged(ref xmlPath, value);
			}
		}

		/// <summary>
		/// Method name
		/// </summary>
		public string MethodName
		{
			get { return methodName; }
			set
			{
				this.RaiseAndSetIfChanged(ref methodName, value);
			}
		}

		/// <summary>
		/// Selected for operation (generate new xml or generate jquery script)
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				this.RaiseAndSetIfChanged(ref isSelected, value);
				SelectChangedNotify();
			}
		}

		/// <summary>
		/// GetTestRailNumber
		/// </summary>
		public string TestRailNumber
		{
			get => testRailNumber;
		}

		#endregion

		#region Private methods
		private void setTestCaseNumber(string name)
		{
			_ = int.TryParse(name.Split(" ")[0].Trim(new char[] { 'C', 'T' }), out int res);
			testCaseNumber = res;
		}

		private void setTestRailNumber(string value) => testRailNumber = (value.Split(" ")[0]);
		#endregion

		#region Public methods
		public int getTestCaseNumber()
		{
			return testCaseNumber;
		}
		#endregion

		#region event
		public delegate void Notify();
		public event Notify SelectChangedNotify;
		#endregion
	}
}
