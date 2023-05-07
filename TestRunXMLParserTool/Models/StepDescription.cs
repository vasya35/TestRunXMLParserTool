using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

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
		[Reactive] public string Name { get; set; } = string.Empty;

		[Reactive] public bool IsActivate { get; set; }

		[Reactive] public bool IsFirstStep { get; set; }
		
		[Reactive] public bool IsLastStep { get; set; }

		[Reactive] public bool IsNextAcvtive { get; set; }

		public ReactiveCommand<Unit, Unit>? ActivateAction { get; internal set; }

		#endregion
	}
}
