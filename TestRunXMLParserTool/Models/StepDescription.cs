using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace TestRunXMLParserTool.Models
{
	public class StepDescription : ReactiveObject
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
			get => name;
			set
			{
				this.RaiseAndSetIfChanged(ref name, value);
			}
		}

		public bool IsActivate
		{
			get => isActivate;
			set
			{
				this.RaiseAndSetIfChanged(ref isActivate, value);
			}
		}

		public bool IsFirstStep
		{
			get => isFirstStep;
			set
			{
				this.RaiseAndSetIfChanged(ref isFirstStep, value);
			}
		}

		public bool IsLastStep
		{
			get => isLastStep;
			set
			{
				this.RaiseAndSetIfChanged(ref isLastStep, value);
			}
		}

		public bool IsNextAcvtive
		{
			get => isNextAcvtive;
			set
			{
				this.RaiseAndSetIfChanged(ref isNextAcvtive, value);
			}
		}

		public RelayCommand? ActivateAction { get; set; }

		#endregion
	}
}
