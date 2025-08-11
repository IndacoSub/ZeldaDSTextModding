using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BMGRandomizerSharpGUI;

public partial class Form1 : Form
{
	private enum TaskChoice
	{
		ExtractText,
		ReplaceText,
		ViewText,
		RandomizeText,
		SomethingElse
	}

	private TaskChoice CurrentTask
	{
		get;
		set;
	}

	private Panel CreateRowPanel(string labelText, bool withBrowse, bool isCombo,
		out Label labelOut, out Control middleOut, out Button? browseOut)
	{
		// Container
		var row = new Panel
		{
			Height = 32,
			Dock = DockStyle.Top,
			Margin = new Padding(0, 4, 0, 4)
		};

		// Label
		var label = new Label
		{
			AutoSize = true,
			Location = new Point(0, 7),
			Text = labelText
		};

		// Middle control
		Control middle;
		if (isCombo)
		{
			middle = new ComboBox
			{
				Location = new Point(120, 4),
				Size = new Size(560, 23),
				DropDownStyle = ComboBoxStyle.DropDownList,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
			};
		}
		else
		{
			middle = new TextBox
			{
				Location = new Point(120, 4),
				Size = new Size(560, 23),
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
			};
		}

		// Optional browse
		Button? browseButton = null;
		if (withBrowse)
		{
			browseButton = new Button
			{
				Text = "...",
				Size = new Size(30, 23),
				Anchor = AnchorStyles.Top | AnchorStyles.Right
			};
			browseButton.Location = new Point(row.Width - browseButton.Width, 3);
		}

		// Capture locals for resize lambda
		var localMiddle = middle;
		var localBrowse = browseButton;

		row.Resize += (s, e) =>
		{
			if (localBrowse != null)
				localBrowse.Left = row.ClientSize.Width - localBrowse.Width;

			var browseWidth = localBrowse?.Width ?? 0;
			localMiddle.Width = Math.Max(60, row.ClientSize.Width - 120 - browseWidth - 8);
		};

		// Output refs
		labelOut = label;
		middleOut = middle;
		browseOut = browseButton;

		// Add children
		row.Controls.Add(label);
		row.Controls.Add(middle);
		if (browseButton != null)
			row.Controls.Add(browseButton);

		return row;
	}


	// Call this from the constructor after InitializeComponent()
	private void BuildDynamicInputs()
	{
		if (IsInDesignMode())
			return; // Avoid running dynamic logic in the Designer

		// Create the vertical container if you don't already have one
		var tlpInputs = new TableLayoutPanel
		{
			Dock = DockStyle.Top,
			AutoSize = true,
			AutoSizeMode = AutoSizeMode.GrowAndShrink,
			ColumnCount = 1,
			RowCount = 0,
			Padding = new Padding(0),
			Margin = new Padding(0),
			GrowStyle = TableLayoutPanelGrowStyle.AddRows,
		};

		// Remove only the previous rows container (if any), keep everything else
		var oldRows = pnlInputs.Controls
			.OfType<TableLayoutPanel>()
			.FirstOrDefault(t => Equals(t.Tag, "InputsRows"));

		if (oldRows != null)
			pnlInputs.Controls.Remove(oldRows);

		// Add the new rows container at the top of pnlInputs
		tlpInputs.Tag = "InputsRows";
		tlpInputs.Dock = DockStyle.Top;
		pnlInputs.Controls.Add(tlpInputs);
		pnlInputs.Controls.SetChildIndex(tlpInputs, 0);

		// 1) Input .nds
		{
			var row = CreateRowPanel("Input .nds:", withBrowse: true, isCombo: false,
				out lblNds, out
				var mid, out btnBrowseNds);
			txtNds = (TextBox)mid;
			btnBrowseNds.Click += btnBrowseNds_Click;
			tlpInputs.Controls.Add(row);
			pnlNds = row;
		}

		// 2) Language (no browse)
		{
			var row = CreateRowPanel("Language:", withBrowse: false, isCombo: true,
				out lblLanguage, out
				var mid, out _);
			cmbLanguage = (ComboBox)mid;
			cmbLanguage.Items.AddRange(new object[]
			{
				"Japanese",
				"English",
				"French",
				"Italian",
				"German",
				"Spanish",
			});
			cmbLanguage.SelectedIndex = 0;
			tlpInputs.Controls.Add(row);
			pnlLanguage = row; // if you want to toggle visibility via ApplyTask
		}

		// 3) Input folder
		{
			var row = CreateRowPanel("Input folder:", withBrowse: true, isCombo: false,
				out lblInputFolder, out
				var mid, out btnBrowseInputFolder);
			txtInputFolder = (TextBox)mid;
			btnBrowseInputFolder.Click += btnBrowseInputFolder_Click;
			tlpInputs.Controls.Add(row);
			pnlInputFolder = row;
		}

		// 4) Output folder
		{
			var row = CreateRowPanel("Output folder:", withBrowse: true, isCombo: false,
				out lblOutputFolder, out
				var mid, out btnBrowseOutputFolder);
			txtOutputFolder = (TextBox)mid;
			btnBrowseOutputFolder.Click += btnBrowseOutputFolder_Click;
			tlpInputs.Controls.Add(row);
			pnlOutputFolder = row;
		}

		// 5) Other language folder
		{
			var row = CreateRowPanel("Other language folder:", withBrowse: true, isCombo: false,
				out lblOtherLangFolder, out
				var mid, out btnBrowseOtherLangFolder);
			txtOtherLangFolder = (TextBox)mid;
			btnBrowseOtherLangFolder.Click += btnBrowseOtherLangFolder_Click;
			tlpInputs.Controls.Add(row);
			pnlOtherLangFolder = row;
		}

		// 6) Output .nds
		{
			var row = CreateRowPanel("Output .nds:", withBrowse: true, isCombo: false,
				out lblOutputNds, out
				var mid, out btnBrowseOutputNds);
			txtOutputNds = (TextBox)mid;
			btnBrowseOutputNds.Click += btnBrowseOutputNds_Click;
			tlpInputs.Controls.Add(row);
			pnlOutputNds = row;
		}

		// Default visibility (Extract)
		pnlInputFolder.Visible = false;
		pnlOtherLangFolder.Visible = false;
		pnlOutputNds.Visible = false;
	}

	private void ApplyTask(TaskChoice choice)
	{
		CurrentTask = choice;

		// Reset visibility
		pnlNds.Visible = false;
		pnlInputFolder.Visible = false;
		pnlOutputFolder.Visible = false;
		pnlOutputNds.Visible = false;
		pnlOtherLangFolder.Visible = false;

		bool isReplace = CurrentTask == TaskChoice.ReplaceText // or your enum name
			||
			CurrentTask == TaskChoice.RandomizeText;

		chkReplaceText.Visible = isReplace;
		chkReplaceGraphics.Visible = isReplace;
		chkReplaceFonts.Visible = isReplace;

		switch (choice)
		{
			case TaskChoice.ExtractText:
				pnlNds.Visible = true;
				pnlOutputFolder.Visible = true;
				break;

			case TaskChoice.ReplaceText:
				pnlNds.Visible = true;
				pnlInputFolder.Visible = true;

				// Show checkboxes for what to replace
				chkReplaceText.Visible = true;
				chkReplaceGraphics.Visible = true;
				chkReplaceFonts.Visible = true;

				// Default to enabled
				chkReplaceText.Checked = true;
				chkReplaceGraphics.Checked = true;
				chkReplaceFonts.Checked = true;
				break;

			case TaskChoice.ViewText:
				pnlNds.Visible = true;
				break;

			case TaskChoice.RandomizeText:
				pnlNds.Visible = true;
				pnlOutputNds.Visible = true;
				break;

			case TaskChoice.SomethingElse:
				break;
		}

		txtLog.Clear();
		progressBar.Value = 0;
	}

	// Safer design-time check than DesignMode in constructor
	private bool IsInDesignMode()
	{
		return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
			(Site?.DesignMode ?? false);
	}
}