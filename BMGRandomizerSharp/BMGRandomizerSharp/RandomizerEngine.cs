// BMGRandomizerSharp.Core/RandomizerEngine.cs
using System.Text;
using System.Text.RegularExpressions;

namespace BMGRandomizerSharp
{
	public sealed class RandomizerOptions
	{
		public string SourceFolder
		{
			get;
		} // still readonly, but set via constructor

		public string? SecondFolder
		{
			get;
			init;
		}
		public Encoding Encoding
		{
			get;
			init;
		} = Encoding.Unicode;
		public int? Seed
		{
			get;
			init;
		}
		public bool StrictSecondFolderCountMatch
		{
			get;
			init;
		} = true;
		public IProgress<string>? Log
		{
			get;
			init;
		}
		public IProgress<int>? Progress
		{
			get;
			init;
		}
		public CancellationToken CancellationToken
		{
			get;
			init;
		} =
		default;

		public RandomizerOptions(string sourceFolder)
		{
			SourceFolder = sourceFolder ??
				throw new ArgumentNullException(nameof(sourceFolder));
		}
	}


	public sealed class RandomizerResult
	{
		public int FilesProcessed
		{
			get;
			init;
		}
		public long LinesProcessed
		{
			get;
			init;
		}
	}

	public static class RandomizerEngine
	{
		private static readonly Regex EscapeRegex = new(@"\[\d+:[a-fA-F0-9]+\]", RegexOptions.Compiled);

		// Important escape sequences that must be preserved during randomization
		private static readonly HashSet<string> NecessaryESs = new()
			{
				"[0:0000]", // Option 1 (ST)
                "[0:0100]", // Option 2 (ST)
                "[0:0200]", // Option 3 (ST)
                "[0:0f001e00]", // Exiting from shops

                "[1:0000]", // Option 1 (PH)
                "[1:0100]", // Option 2 (PH)
                "[1:0200]", // Option 3 (PH)

                "[1:1a000100]", // Northwestern Sea chart
                "[1:1a000200]", // Southeastern Sea chart
                "[1:1a000300]", // Northeastern Sea chart
                "[1:1a000000]", // Southwestern Sea chart?

                "[1:17000000]", // Show current map (Southwestern Sea chart)
                "[3:fe000700]", // ...Hide current map?

                "[0:0f00af00]", // Cutscene text?
            };

		public static async Task<RandomizerResult> RandomizeAsync(RandomizerOptions options)
		{
			var ct = options.CancellationToken;
			var log = options.Log;
			var progress = options.Progress;
			var rnd = options.Seed.HasValue ? new Random(options.Seed.Value) : new Random();

			log?.Report($"[INFO] Source: {options.SourceFolder}");
			if (!string.IsNullOrWhiteSpace(options.SecondFolder))
				log?.Report($"[INFO] Second: {options.SecondFolder}");
			else
				log?.Report("[INFO] Second: (none — single-folder mode)");

			// Load primary files
			var primaryFiles = Directory.GetFiles(options.SourceFolder, "*.*", SearchOption.AllDirectories);
			var filesPath = new List<string>(primaryFiles.Length);
			var filesContent = new List<List<string>>(primaryFiles.Length);

			long totalLines = 0;
			for (int i = 0; i < primaryFiles.Length; i++)
			{
				ct.ThrowIfCancellationRequested();
				var path = primaryFiles[i];
				filesPath.Add(path);
				var lines = new List<string>();
				using
				var sr = new StreamReader(path, options.Encoding, detectEncodingFromByteOrderMarks: true);
				while (!sr.EndOfStream)
				{
					var temp = (await sr.ReadLineAsync().WaitAsync(ct))?.Trim() ?? "";
					lines.Add(temp);
					totalLines++;
				}
				filesContent.Add(lines);
				log?.Report($"[LOAD] {path} loaded with {lines.Count} lines");
				progress?.Report((int)((i + 1) * 10.0 / primaryFiles.Length)); // small pre-progress
			}

			// Optional second folder
			var secondFolderContent = new List<List<string>>();
			if (!string.IsNullOrWhiteSpace(options.SecondFolder))
			{
				var secondFiles = Directory.GetFiles(options.SecondFolder!, "*.*", SearchOption.AllDirectories);
				if (options.StrictSecondFolderCountMatch && secondFiles.Length != primaryFiles.Length)
				{
					log?.Report("[ERROR] Mismatched file count between folders!");
					throw new InvalidOperationException("Second folder file count mismatch.");
				}

				for (int i = 0; i < secondFiles.Length; i++)
				{
					ct.ThrowIfCancellationRequested();
					var path = secondFiles[i];
					var lines = new List<string>();
					using
					var sr = new StreamReader(path, detectEncodingFromByteOrderMarks: true);
					while (!sr.EndOfStream)
					{
						var temp = (await sr.ReadLineAsync().WaitAsync(ct))?.Trim() ?? "";
						lines.Add(temp);
						totalLines++;
					}
					secondFolderContent.Add(lines);
					log?.Report($"[LOAD] {path} from second folder loaded with {lines.Count} lines");
				}
			}

			log?.Report("[INFO] Beginning line-by-line randomization...");
			int fileCount = filesContent.Count;

			for (int i = 0; i < fileCount; i++)
			{
				ct.ThrowIfCancellationRequested();
				var targetFile = filesContent[i];

				for (int j = 0; j < targetFile.Count; j++)
				{
					ct.ThrowIfCancellationRequested();

					var current = targetFile[j];
					if (string.IsNullOrWhiteSpace(current))
						continue;

					bool protectCurrent = ContainsEscapeSequences(current) && !CanSkipEscapeAllSequences(current);
					if (protectCurrent)
					{
						log?.Report($"[PROTECT] Line {j} in {filesPath[i]} preserved due to escape sequence");
						continue;
					}

					// Find a swap line
					string swap = "";
					int tries = 0;
					do
					{
						if (++tries > 1_000_000)
							throw new InvalidOperationException("Too many attempts to find valid swap line.");

						var sourceIsSecond = secondFolderContent.Count > 0 && rnd.Next(0, 2) == 1;
						var listIndex = rnd.Next(0, fileCount);
						var sourceList = sourceIsSecond ? secondFolderContent.ElementAtOrDefault(listIndex) : filesContent[listIndex];
						if (sourceList is null || sourceList.Count == 0) continue;

						var lineIndex = rnd.Next(0, sourceList.Count);
						swap = sourceList[lineIndex];

						if (string.IsNullOrWhiteSpace(swap))
							continue;

						var protectSwap = ContainsEscapeSequences(swap) && !CanSkipEscapeAllSequences(swap);
						if (protectSwap) swap = "";
						else if (listIndex == i && lineIndex == j) swap = ""; // avoid no-op
					}
					while (string.IsNullOrEmpty(swap));

					//log?.Report($"[SWAP] {Path.GetFileName(filesPath[i])} line {j}");
					targetFile[j] = swap;
				}

				progress?.Report(10 + (int)((i + 1) * 85.0 / fileCount)); // bulk progress
			}

			// Write back
			log?.Report("[INFO] Writing randomized files back to disk...");
			for (int k = 0; k < filesPath.Count; k++)
			{
				ct.ThrowIfCancellationRequested();
				var fpath = filesPath[k];
				File.Delete(fpath);
				using
				var sw = new StreamWriter(fpath, append: false, options.Encoding);
				foreach (var line in filesContent[k])
					await sw.WriteLineAsync(line).WaitAsync(ct);

				log?.Report($"[WRITE] {fpath} written with {filesContent[k].Count} lines");
				progress?.Report(95 + (int)((k + 1) * 5.0 / filesPath.Count));
			}

			log?.Report("[✅ DONE] Randomization complete.");
			return new RandomizerResult
			{
				FilesProcessed = filesPath.Count,
				LinesProcessed = totalLines
			};
		}

		private static bool ContainsEscapeSequences(string fullLine) => fullLine.Contains('[') && fullLine.Contains(']');

		private static IEnumerable<string> ExtractEscapes(string input) => EscapeRegex.Matches(input).Select(m => m.Value);

		private static bool CanSkipEscapeAllSequences(string currentLine)
		{
			if (string.IsNullOrWhiteSpace(currentLine)) return false;
			var escapes = ExtractEscapes(currentLine);
			if (!escapes.Any()) return true;

			bool allSkippable = true;
			foreach (var esc in escapes)
				allSkippable &= !NecessaryESs.Contains(esc);

			return allSkippable;
		}
	}
}