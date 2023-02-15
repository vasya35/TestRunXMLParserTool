using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestRunXMLParserTool.Models
{
	public class TestCaseResultModel : INotifyPropertyChanged
	{
		#region fields

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
				if (value != null)
				{
					name = value;
					setTestCaseNumber(value);
					setTestRailNumber(value);
					OnPropertyChanged("Name");
				}
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
				if (value != null)
				{
					result = value;
					OnPropertyChanged("Result");
				}
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
				xmlPath = value;
				OnPropertyChanged("XMLPath");
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
				methodName = value;
				OnPropertyChanged("MethodName");
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
				isSelected = value;
				OnPropertyChanged("IsSelected");
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

		#region implement INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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
	}
}
