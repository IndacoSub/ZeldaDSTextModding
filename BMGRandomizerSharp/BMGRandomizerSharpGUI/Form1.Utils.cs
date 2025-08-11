using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BMGRandomizerSharpGUI;

public partial class Form1 : Form
{
	private bool ValidateInputs()
	{
		if (pnlNds.Visible && string.IsNullOrWhiteSpace(txtNds.Text))
		{
			MessageBox.Show(this, "Please select the input .nds file.", "Missing input",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		if (pnlInputFolder.Visible && string.IsNullOrWhiteSpace(txtInputFolder.Text))
		{
			MessageBox.Show(this, "Please select the input folder.", "Missing input",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		// Output folder is optional for ExtractText; keep validation off in that case
		if (pnlOutputFolder.Visible && CurrentTask != TaskChoice.ExtractText &&
			string.IsNullOrWhiteSpace(txtOutputFolder.Text))
		{
			MessageBox.Show(this, "Please select the output folder.", "Missing input",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		if (pnlOtherLangFolder.Visible && string.IsNullOrWhiteSpace(txtOtherLangFolder.Text))
		{
			MessageBox.Show(this, "Please select the other language folder.", "Missing input",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		if (pnlOutputNds.Visible && string.IsNullOrWhiteSpace(txtOutputNds.Text))
		{
			MessageBox.Show(this, "Please select the output .nds file.", "Missing input",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		return true;
	}


	private void SetUiBusy(bool busy)
	{
		grpTasks.Enabled = !busy;
		pnlInputs.Enabled = !busy;
		btnRun.Enabled = !busy;
		btnCancel.Enabled = busy;
		UseWaitCursor = busy;
	}

	// ——— Task runners ———

	private static string DetectZeldaGame(string ndsPath)
	{
		byte[] header = new byte[16];
		using (var fs = new FileStream(ndsPath, FileMode.Open, FileAccess.Read))
		{
			fs.Read(header, 0, 16);
		}

		string headerStr = Encoding.ASCII.GetString(header);

		if (headerStr.Contains("SPIRITTRACKS"))
			return "Zelda: Spirit Tracks";
		else if (headerStr.Contains("ZELDA_DS:PH"))
			return "Zelda: Phantom Hourglass";
		else
		{
			MessageBox.Show(
                $"Unknown game. First 16 bytes:\n{headerStr}",
				"Unknown ROM",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning
			);
			return "Unknown";
		}
	}

	// ——— Paths and helpers ———

	// Point this to your tools directory that contains the .py and .bat files.
	// You can make this configurable later (Settings dialog).
	// Always resolves to the app’s bin folder at runtime, e.g. ...\bin\Debug\net8.0-windows8.0\
	private static string AppBin => AppContext.BaseDirectory;

	// Where we expect the scripts at runtime: ...\bin\...\tools
	private static string ToolsRoot()
	{
		return Path.Combine(AppBin, "tools");
	}

	private static string ToolPath(string scriptName)
	{
		// Primary path in output folder
		var primary = Path.Combine(ToolsRoot(), scriptName);
		if (File.Exists(primary)) return primary;

		// Fallbacks for dev-time runs (from project root)
		var fallbacks = new[]
		{
            // GUI project root/tools
            Path.GetFullPath(Path.Combine(AppBin, "..", "..", "..", "tools", scriptName)),
                // Current working directory/tools
                Path.Combine(Environment.CurrentDirectory, "tools", scriptName),
		};

		foreach (var f in fallbacks)
			if (File.Exists(f)) return f;

		// Final helpful error
		throw new FileNotFoundException($"Python script not found: {primary}", primary);
	}

	private static string ToolPathEither(string preferred, string alternate)
	{
		try
		{
			return ToolPath(preferred);
		}
		catch (FileNotFoundException)
		{
			return ToolPath(alternate);
		}
	}

	private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
	{
		Directory.CreateDirectory(target.FullName);
		foreach (var f in source.GetFiles())
			f.CopyTo(Path.Combine(target.FullName, f.Name), overwrite: true);
		foreach (var dir in source.GetDirectories())
			CopyAll(dir, new DirectoryInfo(Path.Combine(target.FullName, dir.Name)));
	}

	private static string? FindMostRecentNds(string root)
	{
		var files = Directory.EnumerateFiles(root, "*.nds", SearchOption.AllDirectories);
		return files.Select(p => new FileInfo(p)).OrderByDescending(f => f.LastWriteTimeUtc).FirstOrDefault()?.FullName;
	}



	private static string ComputeDefaultOutputPath(string inputRomPath, string suffix)
	{
		var dir = Path.GetDirectoryName(inputRomPath) ?? AppContext.BaseDirectory;
		var name = Path.GetFileNameWithoutExtension(inputRomPath);
		return Path.Combine(dir, $"{name}{suffix}.nds");
	}

	private static void CopyEditedRomToDestination(string editedRomPath, string destinationRomPath, IProgress<string> log)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(destinationRomPath)!);
		File.Copy(editedRomPath, destinationRomPath, overwrite: true);
		log.Report($"[WRITE] Output ROM saved to: {destinationRomPath}");
	}

	// Some repos have ReplaceFileInROM spelled two ways; pick whichever exists.
	private static string ToolPathEither(params string[] candidates)
	{
		foreach (var c in candidates)
		{
			var p = ToolPath(c);
			if (File.Exists(p)) return p;
		}
		// Fallback to first, even if missing (so caller throws a clear FileNotFound)
		return ToolPath(candidates[0]);
	}
}