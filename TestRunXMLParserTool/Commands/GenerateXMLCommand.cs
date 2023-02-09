using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TestRunXMLParserTool.Commands
{
	public class GenerateXMLCommand : ICommand
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
			var selectedTestCases = (ObservableCollection<TestCaseResultModel>)parameter;
			var xmlGenerator = new GenerateXMLModel();
			xmlGenerator.Generate(selectedTestCases);
		}
	}
}
