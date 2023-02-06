using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestRunXMLParserTool
{
	public class TestCaseResultModel : INotifyPropertyChanged
	{
		private string? name;
		private int ticketNumber;
		private string? result;
		private int? duration;
		private DateTime startedAt;
		private DateTime finishedAt;
		private bool isSelected;

		/// <summary>
		/// Name
		/// </summary>
		public string? Name
		{
			get => name;
			set
			{
				name = value;
				setTestCaseNumber(value);
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Result: PASS, FAIL, Undefined
		/// </summary>
		public string? Result
		{
			get { return result; }
			set
			{
				result = value;
				OnPropertyChanged("Result");
			}
		}

		/// <summary>
		/// Duration in ms
		/// </summary>
		public int? Duration
		{
			get { return duration; }
			set
			{
				duration = value;
				OnPropertyChanged("Duration");
			}
		}

		/// <summary>
		/// Started at 
		/// </summary>
		public DateTime StartedAt
		{
			get { return startedAt; }
			set
			{
				startedAt = value;
				OnPropertyChanged("StartedAt");
			}
		}

		/// <summary>
		/// Finished at 
		/// </summary>
		public DateTime FinishedAt
		{
			get { return finishedAt; }
			set
			{
				finishedAt = value;
				OnPropertyChanged("FinishedAt");
			}
		}

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		public static implicit operator TestCaseResultModel(ObservableCollection<TestCaseResultModel> v)
		{
			throw new NotImplementedException();
		}

		private void setTestCaseNumber(string name)
		{
			int res;
			int.TryParse(name.Split(" ")[0].Trim(new char[] { 'C', 'T' }), out res);
			ticketNumber = res;
		}

		public int getTestCaseNumber()
		{
			return ticketNumber;
		}
	}
}
