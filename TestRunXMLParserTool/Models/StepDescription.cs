using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestRunXMLParserTool.Models
{
	public class StepDescription : INotifyPropertyChanged
	{
		#region Fields
		private string name = "";
		private bool isActivate;
		private bool isFirstStep;
		private bool isLastStep;
		private bool isNextAcvtive;
		#endregion

		#region Properties
		public string Name
		{
			get { return name; }
			set
			{
				if (value == name) return;
				name = value;
				OnPropertyChanged("Name");
			}
		}

		public bool IsActivate
		{
			get { return isActivate; }
			set
			{
				if (value == isActivate) return;
				isActivate = value;
				OnPropertyChanged("IsActivate");
			}
		}

		public bool IsFirstStep
		{
			get { return isFirstStep; }
			set
			{
				if (value == isFirstStep) return;
				isFirstStep = value;
				OnPropertyChanged("IsFirstStep");
			}
		}

		public bool IsLastStep
		{
			get { return isLastStep; }
			set
			{
				if (value == isLastStep) return;
				isLastStep = value;
				OnPropertyChanged("IsLastStep");
			}
		}

		public bool IsNextAcvtive
		{
			get { return isNextAcvtive; }
			set
			{
				if (value == isNextAcvtive) return;
				isNextAcvtive = value;
				OnPropertyChanged("IsNextAcvtive");
			}
		}

		public RelayCommand? ActivateAction { get; set; }

		#endregion

		#region Implementation INotifyPropertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion
	}
}
