using System.Diagnostics;
using System.Text;

namespace BMGRandomizerSharpGUI;

public sealed class ProcessRunner
{
	// You can make this configurable later
	public string PythonExePath
	{
		get;
		set;
	} = "python";

	public async Task RunPythonAsync(string scriptPath, IEnumerable<string> args, string workingDir, IProgress<string> log, CancellationToken ct)
	{

		if (!File.Exists(scriptPath))
		{
			// Helpful diagnostics
			var dir = Path.GetDirectoryName(scriptPath) ?? "(null)";
			log.Report($"[DEBUG] Looking in: {dir}");
			if (Directory.Exists(dir))
			{
				var files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly)
					.Select(Path.GetFileName);
				log.Report("[DEBUG] Files in tools dir: " + string.Join(", ", files));
			}
			throw new FileNotFoundException("Python script not found: " + scriptPath, scriptPath);
		}

		var argLine = BuildArgLine(new[]
		{
			scriptPath
		}.Concat(args));
		await RunProcessAsync(
			fileName: PythonExePath,
			arguments: argLine,
			workingDir: workingDir,
			log: log,
			env: new Dictionary<string, string?>
			{
				["PYTHONIOENCODING"] = "utf-8",
				["PYTHONUTF8"] = "1"
			},
			ct: ct);
	}


	public async Task RunBatchAsync(string batchPath, IEnumerable<string> args, string workingDir, IProgress<string> log, CancellationToken ct)
	{
		if (!File.Exists(batchPath))
			throw new FileNotFoundException("Batch file not found", batchPath);

		string argLine = "/c \"" + batchPath + "\" " + BuildArgLine(args);
		await RunProcessAsync(
			fileName: "cmd.exe",
			arguments: argLine,
			workingDir: workingDir,
			log: log,
			env: null,
			ct: ct);
	}

	private static async Task RunProcessAsync(
		string fileName,
		string arguments,
		string workingDir,
		IProgress<string> log,
		IDictionary<string, string?>? env,
		CancellationToken ct
	)
	{
		var psi = new ProcessStartInfo
		{
			FileName = fileName,
			Arguments = arguments,
			WorkingDirectory = workingDir,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true,
			StandardOutputEncoding = Encoding.UTF8,
			StandardErrorEncoding = Encoding.UTF8
		};

		if (env is not null)
		{
			foreach (var kv in env)
				psi.Environment[kv.Key] = kv.Value;
		}

		// Log full command and context
		static string QuoteForLog(string s) => string.IsNullOrEmpty(s) ?
			"\"\"" :
			(s.IndexOfAny(new[]
			{
				' ',
				'\t',
				'"'
			}) >= 0 ? "\"" + s.Replace("\"", "\\\"") + "\"" : s);

		var envSummary = (env is not null && env.Count > 0) ?
			string.Join(", ", env.Select(kv => $"{kv.Key}={(kv.Value ?? string.Empty)}")) :
			null;

		var fullCmdForLog = $"{QuoteForLog(psi.FileName)} {(psi.Arguments ?? string.Empty)}".Trim();

		if (!string.IsNullOrEmpty(envSummary))
			log.Report("[ENV] " + envSummary);

		log.Report($"[RUN] (cwd: {psi.WorkingDirectory}) {fullCmdForLog}");

		using
		var proc = new Process
		{
			StartInfo = psi,
			EnableRaisingEvents = true
		};
		var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

		proc.OutputDataReceived += (_, e) =>
		{
			if (e.Data is not null) log.Report(e.Data);
		};
		proc.ErrorDataReceived += (_, e) =>
		{
			if (e.Data is not null) log.Report("[ERR] " + e.Data);
		};
		proc.Exited += (_, __) => tcs.TrySetResult(proc.ExitCode);

		if (!proc.Start())
			throw new InvalidOperationException("Failed to start process.");

		proc.BeginOutputReadLine();
		proc.BeginErrorReadLine();

		using (ct.Register(() =>
		{
			try
			{
				if (!proc.HasExited) proc.Kill(true);
			}
			catch
			{ }
		}))
		{
			int exit = await tcs.Task.ConfigureAwait(false);
			if (exit != 0)
				throw new InvalidOperationException($"Process exited with code {exit}. Command: {fullCmdForLog}");
		}
	}


	private static string BuildArgLine(IEnumerable<string> args)
	{
		return string.Join(" ", args.Select(QuoteIfNeeded));
	}

	private static string QuoteIfNeeded(string s)
	{
		if (string.IsNullOrEmpty(s)) return "\"\"";
		if (s.IndexOfAny(new[]
			{
				' ',
				'\t',
				'"',
				'\'',
				'&'
			}) >= 0)
			return "\"" + s.Replace("\"", "\\\"") + "\"";
		return s;
	}
}