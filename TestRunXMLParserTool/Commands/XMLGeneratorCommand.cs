﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestRunXMLParserTool.Models;

namespace TestRunXMLParserTool.Commands
{
	public class XMLGeneratorCommand : ICommand
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
			if (parameter is not ObservableCollection<TestCaseResultModel> selectedTestCases) return;

			XMLGeneratorModel.Generate(selectedTestCases);
		}
	}
}