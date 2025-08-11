using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace BMGRandomizerSharpGUI
{
	public class ViewTextForm : Form
	{
		private readonly string _rootFolder;
		private readonly SplitContainer _split;
		private readonly ListBox _lstFiles;
		private readonly ListBox _lstLines;

		public ViewTextForm(string folder)
		{
			_rootFolder = folder;
			Text = "View Extracted Text";
			MinimumSize = new Size(800, 500);
			StartPosition = FormStartPosition.CenterParent;

			_split = new SplitContainer
			{
				Dock = DockStyle.Fill,
				Orientation = Orientation.Vertical,
				FixedPanel = FixedPanel.Panel1
			};

			_lstFiles = new ListBox
			{
				Dock = DockStyle.Fill,
				IntegralHeight = false,
				Font = new Font("Segoe UI", 9f, FontStyle.Regular)
			};
			_lstFiles.SelectedIndexChanged += LstFiles_SelectedIndexChanged;

			_lstLines = new ListBox
			{
				Dock = DockStyle.Fill,
				IntegralHeight = false,
				Font = new Font("Consolas", 10f, FontStyle.Regular),

				HorizontalScrollbar = true

			};

			_split.Panel1.Controls.Add(_lstFiles);
			_split.Panel2.Controls.Add(_lstLines);
			Controls.Add(_split);

			LoadFileList();
		}

		private void LoadFileList()
		{
			_lstFiles.Items.Clear();

			if (!Directory.Exists(_rootFolder))
				return;

			var files = Directory
				.EnumerateFiles(_rootFolder, "*.bmg_out", SearchOption.TopDirectoryOnly)
				.Select(p => Path.GetFileName(p))
				.OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
				.ToList();

			foreach (var f in files)
				_lstFiles.Items.Add(f);

			SetPanelWidthToLongestItem();

			if (_lstFiles.Items.Count > 0)
				_lstFiles.SelectedIndex = 0;
		}

		private void SetPanelWidthToLongestItem()
		{
			int maxTextWidth = 0;
			using (Graphics g = CreateGraphics())
			{
				foreach (string item in _lstFiles.Items)
				{
					Size size = TextRenderer.MeasureText(g, item, _lstFiles.Font);
					maxTextWidth = Math.Max(maxTextWidth, size.Width);
				}
			}

			_split.SplitterDistance = maxTextWidth + 20; // 10px padding + room for scrollbar
		}

		private void LstFiles_SelectedIndexChanged(object? sender, EventArgs e)
		{
			_lstLines.Items.Clear();

			if (_lstFiles.SelectedItem is not string fileName)
				return;

			string path = Path.Combine(_rootFolder, fileName);

			try
			{
				// Auto-detect encoding (UTF-8 or UTF-16)
				using
				var sr = new StreamReader(path, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
				string rawContent = sr.ReadToEnd();

				var lines = rawContent
					.Replace("\r\n", "\n")
					.Replace("\r", "\n")
					.Split('\n')
					.Select(line => line.TrimEnd())
					.Where(line => line.Length > 0)
					.ToList();

				_lstLines.Items.Clear();
				for (int i = 0; i < lines.Count; i++)
				{
					string prefix = (i + 1).ToString("D3"); // 001, 002, ...
					_lstLines.Items.Add($"{prefix}: {lines[i]}");
				}
			}
			catch (Exception ex)
			{
				_lstLines.Items.Add("[ERROR] Failed to read file:");
				_lstLines.Items.Add(path);
				_lstLines.Items.Add(ex.Message);
			}
		}
	}
}