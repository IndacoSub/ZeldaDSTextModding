using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BMGRandomizerSharpGUI;

public partial class Form1 : Form
{
	// Task selection
	private void rdo_CheckedChanged(object sender, EventArgs e)
	{
		if (!((RadioButton)sender).Checked) return;

		if (sender == rdoExtract) ApplyTask(TaskChoice.ExtractText);
		else if (sender == rdoReplace) ApplyTask(TaskChoice.ReplaceText);
		else if (sender == rdoView) ApplyTask(TaskChoice.ViewText);
		else if (sender == rdoRandomize) ApplyTask(TaskChoice.RandomizeText);
		else ApplyTask(TaskChoice.SomethingElse);
	}

	// Browse buttons
	private void btnBrowseNds_Click(object sender, EventArgs e)
	{
		using
		var ofd = new OpenFileDialog
		{
			Filter = "Nintendo DS ROM (*.nds)|*.nds|All files (*.*)|*.*",
			Title = "Select input .nds file"
		};
		if (ofd.ShowDialog(this) == DialogResult.OK)
			txtNds.Text = ofd.FileName;
	}

	private void btnBrowseInputFolder_Click(object sender, EventArgs e)
	{
		using
		var dlg = new FolderBrowserDialog
		{
			Description = "Select input folder"
		};
		if (dlg.ShowDialog(this) == DialogResult.OK)
			txtInputFolder.Text = dlg.SelectedPath;
	}

	private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
	{
		using
		var dlg = new FolderBrowserDialog
		{
			Description = "Select output folder"
		};
		if (dlg.ShowDialog(this) == DialogResult.OK)
			txtOutputFolder.Text = dlg.SelectedPath;
	}

	private void btnBrowseOtherLangFolder_Click(object sender, EventArgs e)
	{
		using
		var dlg = new FolderBrowserDialog
		{
			Description = "Select other language files folder"
		};
		if (dlg.ShowDialog(this) == DialogResult.OK)
			txtOtherLangFolder.Text = dlg.SelectedPath;
	}

	private void btnBrowseOutputNds_Click(object sender, EventArgs e)
	{
		using
		var sfd = new SaveFileDialog
		{
			Filter = "Nintendo DS ROM (*.nds)|*.nds|All files (*.*)|*.*",
			Title = "Select output .nds file"
		};
		if (sfd.ShowDialog(this) == DialogResult.OK)
			txtOutputNds.Text = sfd.FileName;
	}

	private void DeleteEditedRomIfPresent()
	{
		var path = Path.Combine(ToolsRoot(), "EditedROM.nds");

		try
		{
			Console.WriteLine("[CLEANUP] Checking for EditedROM.nds...");
			if (File.Exists(path))
			{
				try
				{
					File.SetAttributes(path, FileAttributes.Normal); // Clear ReadOnly
					Console.WriteLine("[CLEANUP] Cleared read-only attribute.");
				}
				catch (Exception attrEx)
				{
					Console.WriteLine($"[WARN] Could not modify file attributes: {attrEx.Message}");
				}

				File.Delete(path);
				txtLog.AppendText("[CLEANUP] Deleted tools/EditedROM.nds\r\n");
				Console.WriteLine("[CLEANUP] Successfully deleted EditedROM.nds.");
			}
			else
			{
				Console.WriteLine("[CLEANUP] EditedROM.nds file not found. No deletion necessary.");
			}
		}
		catch (IOException ioEx)
		{
			Console.WriteLine($"[ERROR] IO exception during cleanup: {ioEx.Message}");
		}
		catch (UnauthorizedAccessException authEx)
		{
			Console.WriteLine($"[ERROR] Unauthorized access during cleanup: {authEx.Message}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[ERROR] Unexpected error during cleanup: {ex.Message}");
		}
	}


	// Run / Cancel
	private async void btnRun_Click(object sender, EventArgs e)
	{
		if (!ValidateInputs()) return;

		SetUiBusy(true);
		_cts = new CancellationTokenSource();

		var log = new Progress<string>(line =>
		{
			txtLog.AppendText(line + Environment.NewLine);
		});

		var percent = new Progress<int>(p =>
		{
			progressBar.Value = Math.Clamp(p, 0, 100);
		});

		try
		{
			switch (CurrentTask)
			{
				case TaskChoice.ExtractText:
					await RunExtractAsync(log, percent, _cts.Token);
					break;
				case TaskChoice.ReplaceText:
					await RunReplaceAsync(log, percent, _cts.Token);
					break;
				case TaskChoice.ViewText:
					await RunViewAsync(log, percent, _cts.Token);
					break;
				case TaskChoice.RandomizeText:
					await RunRandomizeAsync(log, percent, _cts.Token);
					break;
				case TaskChoice.SomethingElse:
					txtLog.AppendText("[INFO] No action defined yet.\r\n");
					break;
			}
		}
		catch (OperationCanceledException)
		{
			txtLog.AppendText("[CANCELLED] Operation cancelled.\r\n");
		}
		catch (Exception ex)
		{
			txtLog.AppendText("[ERROR] " + ex.Message + Environment.NewLine);
			Debug.WriteLine(ex);
			MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		finally
		{
			_cts?.Dispose();
			_cts = null;
			SetUiBusy(false);

			// After everything: clean up EditedROM.nds if present
			DeleteEditedRomIfPresent();
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		_cts?.Cancel();
	}
}