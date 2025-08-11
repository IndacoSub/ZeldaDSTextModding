using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BMGRandomizerSharp;

namespace BMGRandomizerSharp
{

	internal class Program
	{
		/*
		static async Task<int> Main(string[] args)
		{
			// ✅ Parse arguments: accepts 1–2 args
			if (args.Length == 0 || args.Length > 2)
			{
				Console.WriteLine("Usage: BMGRandomizerSharp.exe <source_folder>");
				return 1;
			}

			string source = args[0]; // Primary folder path
			string? second = args.Length > 1 ? args[1] : null; // Optional second folder

			// ✅ Display chosen folders
			Console.WriteLine($"[INFO] Source folder: {source}");

			// ✅ Run the randomizer engine
			try
			{
				var result = await RandomizerEngine.RandomizeAsync(new RandomizerOptions(source)
				{
					SecondFolder = null,
					Log = new Progress<string>(msg => Console.WriteLine(msg)),
					Progress = new Progress<int>(p =>
					{ })
				});


				Console.WriteLine($"[STATS] Files processed: {result.FilesProcessed}, Total lines processed: {result.LinesProcessed}");
				return 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ERROR] {ex.Message}");
				return 2;
			}
		}
		*/
	}
}