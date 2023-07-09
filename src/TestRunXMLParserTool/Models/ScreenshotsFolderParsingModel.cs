using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestRunXMLParserTool.Models
{
	public class ScreenshotsFolderParsingModel
	{
		#region fileds
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		#endregion

		#region Public Methods
		public static async Task<Tuple<bool, string, List<string>>> GetTestCasesByFolderAsync(string path)
		{
			return await Task.Run(() => GetTestCasesByFolder(path));
		}

		public static Tuple<bool, string, List<string>> GetTestCasesByFolder(string path)
		{
			try
			{
				var result = new List<string>();

				if (Directory.Exists(path))
				{
					string[] fileEntries = Directory.GetFiles(path);


					for (int i = 0; i < fileEntries.Length; i++)
					{
						string tmp = GetNumberFromPath(fileEntries[i]);
						if (tmp != "")
						{
							result.Add(tmp);
						}
					}
				}

				return Tuple.Create(false, "", result);
			}
			catch (Exception e)
			{
				Logger.Error($"Error while parsing screenshots folder: {e}");

				return Tuple.Create(false, e.ToString(), new List<string>());
			}
		}
		#endregion

		#region Private methods
		private static string GetNumberFromPath(string filePath)
		{
			string resNumber = "";

			try
			{
				var fileName = filePath.Split("\\").Last();
				var number = fileName.Split(".").First().Split("_").First();


				foreach (var item in number)
				{
					if (char.IsDigit(item))
					{
						resNumber += item.ToString();
					}
				}
			}
			catch (Exception e)
			{
				Logger.Warn($"Error while parsing path for file {filePath}: {e}");
			}

			return resNumber;
		}
		#endregion
	}
}
