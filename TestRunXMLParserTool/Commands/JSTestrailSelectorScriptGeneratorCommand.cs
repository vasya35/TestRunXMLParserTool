using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.Commands
{
	internal class JSTestrailSelectorScriptGeneratorCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object? parameter)
		{
			return true;
		}

		public void Execute(object? parameter)
		{
			var selectedTestCases = parameter as ObservableCollection<TestCaseResultModel>;
			if (selectedTestCases == null) return;			

			var jqueryScriptGenerator = new JSTestRailSelectorScriptGeneratorModel();
			jqueryScriptGenerator.Generate(selectedTestCases);
		}
	}
}
