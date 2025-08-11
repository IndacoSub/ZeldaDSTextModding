using System;
using System.Drawing;
using System.Windows.Forms;

namespace BMGRandomizerSharpGUI
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;

		// Top task chooser
		private GroupBox grpTasks;
		private FlowLayoutPanel flpTasks;
		private RadioButton rdoExtract;
		private RadioButton rdoReplace;
		private RadioButton rdoView;
		private RadioButton rdoRandomize;
		private RadioButton rdoOther;

		// Inputs area (populated at runtime in Form1.cs)
		private Panel pnlInputs;
		private Panel pnlReplaceOptions;

		private CheckBox chkReplaceText;
		private CheckBox chkReplaceGraphics;
		private CheckBox chkReplaceFonts;

		// Placeholders for runtime-created rows/controls (assigned in Form1.cs)
		private Panel pnlNds;
		private Label lblNds;
		private TextBox txtNds;
		private Button btnBrowseNds;

		private Panel pnlLanguage;
		private Label lblLanguage;
		private ComboBox cmbLanguage;

		private Panel pnlInputFolder;
		private Label lblInputFolder;
		private TextBox txtInputFolder;
		private Button btnBrowseInputFolder;

		private Panel pnlOutputFolder;
		private Label lblOutputFolder;
		private TextBox txtOutputFolder;
		private Button btnBrowseOutputFolder;

		private Panel pnlOtherLangFolder;
		private Label lblOtherLangFolder;
		private TextBox txtOtherLangFolder;
		private Button btnBrowseOtherLangFolder;

		private Panel pnlOutputNds;
		private Label lblOutputNds;
		private TextBox txtOutputNds;
		private Button btnBrowseOutputNds;

		// Bottom area
		private TableLayoutPanel tlpBottom;
		private ProgressBar progressBar;
		private FlowLayoutPanel flpBottomButtons;
		private Button btnRun;
		private Button btnCancel;

		// Central log
		private TextBox txtLog;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();

			// ============== Top: Task group ==============
			grpTasks = new GroupBox();
			flpTasks = new FlowLayoutPanel();
			rdoExtract = new RadioButton();
			rdoReplace = new RadioButton();
			rdoView = new RadioButton();
			rdoRandomize = new RadioButton();
			rdoOther = new RadioButton();

			grpTasks.Text = "What do you want to do?";
			grpTasks.Dock = DockStyle.Top;
			grpTasks.Padding = new Padding(10);
			grpTasks.Height = 82;

			flpTasks.Dock = DockStyle.Fill;
			flpTasks.FlowDirection = FlowDirection.LeftToRight;
			flpTasks.WrapContents = true;
			flpTasks.AutoSize = true;
			flpTasks.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			rdoExtract.Text = "Extract text";
			rdoExtract.AutoSize = true;
			rdoExtract.Checked = true;
			rdoExtract.Margin = new Padding(3, 3, 12, 3);
			rdoExtract.CheckedChanged += rdo_CheckedChanged;

			rdoReplace.Text = "Replace text";
			rdoReplace.AutoSize = true;
			rdoReplace.Margin = new Padding(3, 3, 12, 3);
			rdoReplace.CheckedChanged += rdo_CheckedChanged;

			rdoView.Text = "View text files";
			rdoView.AutoSize = true;
			rdoView.Margin = new Padding(3, 3, 12, 3);
			rdoView.CheckedChanged += rdo_CheckedChanged;

			rdoRandomize.Text = "Randomize text";
			rdoRandomize.AutoSize = true;
			rdoRandomize.Margin = new Padding(3, 3, 12, 3);
			rdoRandomize.CheckedChanged += rdo_CheckedChanged;

			rdoOther.Text = "Something else";
			rdoOther.AutoSize = true;
			rdoOther.Margin = new Padding(3, 3, 12, 3);
			rdoOther.CheckedChanged += rdo_CheckedChanged;

			flpTasks.Controls.AddRange(new Control[]
			{
				rdoExtract, rdoReplace, rdoView, rdoRandomize, rdoOther
			});
			grpTasks.Controls.Add(flpTasks);

			// ============== Middle: Inputs container (rows added at runtime) ==============
			pnlInputs = new Panel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Padding = new Padding(12, 6, 12, 6)
			};

			// Panel for checkbox group
			pnlReplaceOptions = new Panel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Padding = new Padding(6, 6, 6, 0),
				Visible = true
			};

			chkReplaceText = new CheckBox
			{
				Text = "Replace Text",
				AutoSize = true,
				Dock = DockStyle.Top
			};

			chkReplaceGraphics = new CheckBox
			{
				Text = "Replace Graphics",
				AutoSize = true,
				Dock = DockStyle.Top
			};

			chkReplaceFonts = new CheckBox
			{
				Text = "Replace Fonts",
				AutoSize = true,
				Dock = DockStyle.Top
			};

			pnlReplaceOptions.Controls.Add(chkReplaceFonts);
			pnlReplaceOptions.Controls.Add(chkReplaceGraphics);
			pnlReplaceOptions.Controls.Add(chkReplaceText);

			pnlInputs.Controls.Add(pnlReplaceOptions);


			// ============== Center: Log ==============
			txtLog = new TextBox
			{
				Dock = DockStyle.Fill,
				Multiline = true,
				ScrollBars = ScrollBars.Vertical,
				ReadOnly = true,
				WordWrap = false,
				BorderStyle = BorderStyle.FixedSingle
			};

			// ============== Bottom: Progress + buttons ==============
			tlpBottom = new TableLayoutPanel
			{
				Dock = DockStyle.Bottom,
				Height = 44,
				ColumnCount = 2,
				RowCount = 1,
				Padding = new Padding(12, 8, 12, 8)
			};
			tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			progressBar = new ProgressBar
			{
				Dock = DockStyle.Fill,
				Minimum = 0,
				Maximum = 100
			};

			flpBottomButtons = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = false,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Dock = DockStyle.Fill
			};

			btnRun = new Button
			{
				Text = "Run",
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Margin = new Padding(6, 0, 0, 0)
			};
			btnRun.Click += btnRun_Click;

			btnCancel = new Button
			{
				Text = "Cancel",
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Enabled = false,
				Margin = new Padding(6, 0, 0, 0)
			};
			btnCancel.Click += btnCancel_Click;

			flpBottomButtons.Controls.Add(btnRun);
			flpBottomButtons.Controls.Add(btnCancel);

			tlpBottom.Controls.Add(progressBar, 0, 0);
			tlpBottom.Controls.Add(flpBottomButtons, 1, 0);

			// ============== Form ==============
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(900, 600);
			MinimumSize = new Size(720, 480);
			Text = "BMG Randomizer GUI";

			// Top-to-bottom stacking: Fill in between
			Controls.Add(txtLog);
			Controls.Add(tlpBottom);
			Controls.Add(pnlInputs);
			Controls.Add(grpTasks);

			ResumeLayout(false);
		}
	}
}
