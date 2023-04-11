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
				name = value;
				OnPropertyChanged("Name");
			}
		}

		public bool IsActivate
		{
			get { return isActivate; }
			set
			{
				isActivate = value;
				OnPropertyChanged("IsActivate");
			}
		}

		public bool IsFirstStep
		{
			get { return isFirstStep; }
			set
			{
				isFirstStep = value;
				OnPropertyChanged("IsFirstStep");
			}
		}

		public bool IsLastStep
		{
			get { return isLastStep; }
			set
			{
				isLastStep = value;
				OnPropertyChanged("IsLastStep");
			}
		}

		public bool IsNextAcvtive
		{
			get { return isNextAcvtive; }
			set
			{
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
