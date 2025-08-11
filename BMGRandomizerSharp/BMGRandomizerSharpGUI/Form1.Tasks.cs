using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

using BMGRandomizerSharp;

namespace BMGRandomizerSharpGUI;

public partial class Form1 : Form
{
	private async Task RunExtractAsync(IProgress<string> log, IProgress<int> percent, CancellationToken ct)
	{
		string nds = txtNds.Text.Trim();
		// If user left output folder blank, pass empty string to trigger script default
		string outFolderArg = string.IsNullOrWhiteSpace(txtOutputFolder.Text) ?
			"" :
			txtOutputFolder.Text.Trim();

		// Read language from combo, defaulting to Japanese
		string language = (cmbLanguage?.SelectedItem as string) ?? "Japanese";

		// Optionally pre-create folder if provided (not needed when blank/default)
		if (!string.IsNullOrWhiteSpace(outFolderArg))
			Directory.CreateDirectory(outFolderArg);

		percent.Report(0);

		await _runner.RunPythonAsync(
			scriptPath: ToolPath("SaveAllStringsFromFile.py"),
			args: new[]
			{
				nds,
				outFolderArg,
				language
			},
			workingDir: ToolsRoot(),
			log: log,
			ct: ct);

		percent.Report(100);
	}

	private async Task RunReplaceAsync(IProgress<string> log, IProgress<int> percent, CancellationToken ct)
	{
		percent.Report(0);

		string inputRom = txtNds.Text.Trim();
		string editFolder = txtInputFolder.Text.Trim();
		string outRom = ComputeDefaultOutputPath(inputRom, "_replaced");

		await RunRebuildAsync(
			inputRomPath: inputRom,
			editFolderPath: editFolder,
			replaceText: chkReplaceText.Checked,
			replaceGraphics: chkReplaceGraphics.Checked,
			replaceFonts: chkReplaceFonts.Checked,
			log: log,
			ct: ct);

		string editedRom = Path.Combine(ToolsRoot(), "EditedROM.nds");
		CopyEditedRomToDestination(editedRom, outRom, log);

		percent.Report(100);
	}

	private async Task RunViewAsync(IProgress<string> log, IProgress<int> percent, CancellationToken ct)
	{
		percent.Report(0);

		string tempOut = Path.Combine(Path.GetTempPath(), "BMGRandomizer_View", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
		Directory.CreateDirectory(tempOut);

		// Optional: pass language if you have the ComboBox
		string language = (cmbLanguage?.SelectedItem as string) ?? "Japanese";

		try
		{
			// Extract .bmg_out files to tempOut
			await _runner.RunPythonAsync(
				scriptPath: ToolPath("SaveAllStringsFromFile.py"),
				args: new[]
				{
					txtNds.Text.Trim(), tempOut, language
				},
				workingDir: ToolsRoot(),
				log: log,
				ct: ct);

			percent.Report(60);
			ct.ThrowIfCancellationRequested();

			// Show viewer (modal) and wait until user closes it
			using (var viewer = new ViewTextForm(tempOut))
			{
				viewer.ShowInTaskbar = false;
				viewer.StartPosition = FormStartPosition.CenterParent;
				viewer.ShowDialog(this);
			}

			percent.Report(90);
		}
		finally
		{
			// Always try to clean up temp
			try
			{
				if (Directory.Exists(tempOut)) Directory.Delete(tempOut, recursive: true);
			}
			catch (Exception ex)
			{
				log.Report("[WARN] Could not delete temp folder: " + ex.Message);
			}
			percent.Report(100);
		}
	}

	private async Task RunRandomizeAsync(IProgress<string> log, IProgress<int> percent, CancellationToken ct)
	{
		percent.Report(0);

		string inputRom = txtNds.Text.Trim();
		string outRom = string.IsNullOrWhiteSpace(txtOutputNds.Text) ?
			ComputeDefaultOutputPath(inputRom, "_randomized") :
			txtOutputNds.Text.Trim();

		string tempRoot = Path.Combine(Path.GetTempPath(), "BMGRandomizer_Randomize", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
		string tempExtract = Path.Combine(tempRoot, "extract");
		Directory.CreateDirectory(tempExtract);

		// Read language from combo, defaulting to Japanese
		string language = (cmbLanguage?.SelectedItem as string) ?? "Japanese";

		// 1) Extract
		await _runner.RunPythonAsync(
			scriptPath: ToolPath("SaveAllStringsFromFile.py"),
			args: new[]
			{
				inputRom,
				tempExtract,
				language
			},
			workingDir: ToolsRoot(),
			log: log,
			ct: ct);
		percent.Report(33);

		// 2) Randomize using the new shared engine
		await RandomizerEngine.RandomizeAsync(new RandomizerOptions(tempExtract)
		{
			Log = log,
			Progress = new Progress<int>(p => percent.Report(33 + (int)(p * 0.33))), // maps 33..66
			CancellationToken = ct
		});


		percent.Report(66);

		// 3) Rebuild text
		await RunRebuildAsync(
			inputRomPath: inputRom,
			editFolderPath: tempExtract,
			replaceText: chkReplaceText.Checked,
			replaceGraphics: false,
			replaceFonts: false,
			log: log,
			ct: ct);

		// Copy EditedROM.nds to final destination
		string editedRom = Path.Combine(ToolsRoot(), "EditedROM.nds");
		CopyEditedRomToDestination(editedRom, outRom, log);

		percent.Report(100);
	}

	private async Task RunRebuildAsync(
		string inputRomPath,
		string editFolderPath,
		bool replaceText,
		bool replaceGraphics,
		bool replaceFonts,
		IProgress<string> log,
		CancellationToken ct)
	{
		string workingDir = ToolsRoot();
		Directory.CreateDirectory(workingDir);

		string editedRom = Path.Combine(workingDir, "EditedROM.nds");
		File.Copy(inputRomPath, editedRom, overwrite: true);
		log.Report("[COPY] Editable ROM created.");

		string gameName = DetectZeldaGame(inputRomPath);
		log.Report($"[GAME] Detected: {gameName}");

		string[] messageFiles = Array.Empty<string>();
		var graphicMap = Array.Empty<(string Local, string RomPath)>();
		var fontMap = Array.Empty<(string Local, string RomPath)>();

		string language = (cmbLanguage?.SelectedItem as string) ?? "Japanese";

		if (gameName.Contains("Spirit Tracks", StringComparison.OrdinalIgnoreCase))
		{
			messageFiles = new[]
			{
				"battle_common",
				"battle_parent",
				"castle",
				"castle_town",
				"collect",
				"demo",
				"demo01_05",
				"demo06_10",
				"demo11_15",
				"demo16_20",
				"demo21_25",
				"desert",
				"dungeon",
				"field",
				"flame",
				"flame_fld",
				"forest",
				"intrain",
				"maingame",
				"post",
				"regular",
				"select",
				"shop",
				"snow",
				"tower",
				"tower_lobby",
				"train",
				"village",
				"water"
			};

			graphicMap = new[]
			{
				("mixedLarge.bin", language + "/libASR/mixedLarge.bin"),
				("tex2d.bin", language + "/Screen/tex2d.bin"),
				("KeyboardBG.bin", language + "/Screen/Bg/KeyboardBG.bin"),
				("BattleM.bin", language + "/Screen/Layout/BattleM.bin"),
				("BselM_C.bin", language + "/Screen/Layout/BselM_C.bin"),
				("BselS_A.bin", language + "/Screen/Layout/BselS_A.bin"),
				("Cargo.ncgr", language + "/Screen/Layout/Cargo.ncgr"),
				("CmnM.bin", language + "/Screen/Layout/CmnM.bin"),
				("CmnM_L.bin", language + "/Screen/Layout/CmnM_L.bin"),
				("CmnS.bin", language + "/Screen/Layout/CmnS.bin"),
				("CollectM.bin", language + "/Screen/Layout/CollectM.bin"),
				("CollectS.bin", language + "/Screen/Layout/CollectS.bin"),
				("Face.ncgr", language + "/Screen/Layout/Face.ncgr"),
				("Fin.bin", language + "/Screen/Layout/Fin.bin"),
				("Item_L.ncgr", language + "/Screen/Layout/Item_L.ncgr"),
				("Keyboard.bin", language + "/Screen/Layout/Keyboard.bin"),
				("LandM.bin", language + "/Screen/Layout/LandM.bin"),
				("LandM_L.bin", language + "/Screen/Layout/LandM_L.bin"),
				("LetterM.bin", language + "/Screen/Layout/LetterM.bin"),
				("ListS.bin", language + "/Screen/Layout/ListS.bin"),
				("MiniM.bin", language + "/Screen/Layout/MiniM.bin"),
				("OverS.bin", language + "/Screen/Layout/OverS.bin"),
				("Rabbit.bin", language + "/Screen/Layout/Rabbit.bin"),
				("ResultS.bin", language + "/Screen/Layout/ResultS.bin"),
				("StampM.bin", language + "/Screen/Layout/StampM.bin"),
				("TitleM.bin", language + "/Screen/Layout/TitleM.bin"),
				("TrainS.bin", language + "/Screen/Layout/TrainS.bin"),
				("WXCS.bin", language + "/Screen/Layout/WXCS.bin")
			};

			fontMap = new[]
			{
				("DSZ2_endL.nftr", "Font/DSZ2_endL.nftr"),
				("DSZ2_endS.nftr", "Font/DSZ2_endS.nftr"),
				("DSZ2_msg.nftr", "Font/DSZ2_msg.nftr"),
				("LC_Font_s_Name.nftr", "Font/LC_Font_s_Name.nftr")
			};
		}
		else if (gameName.Contains("Phantom Hourglass", StringComparison.OrdinalIgnoreCase))
		{
			messageFiles = new[]
			{
				"battle",
				"battleCommon",
				"bossLast1",
				"bossLast3",
				"brave",
				"collect",
				"demo",
				"field",
				"flame",
				"frost",
				"ghost",
				"hidari",
				"kaitei",
				"kaitei_F",
				"kojima1",
				"kojima2",
				"kojima3",
				"kojima5",
				"mainselect",
				"main_isl",
				"myou",
				"power",
				"regular",
				"sea",
				"sennin",
				"ship",
				"staff",
				"system",
				"torii",
				"wind",
				"wisdom",
				"wisdom_dngn"
			};

			graphicMap = new[]
			{
				("btlPlBg.bin", language + "/Menu/Bg/btlPlBg.bin"),
				("Color0.NCLR", language + "/Menu/Bg/Color0.NCLR"),
				("endlogoS.bin", language + "/Menu/Bg/endlogoS.bin"),
				("frameBg.bin", language + "/Menu/Bg/frameBg.bin"),
				("pslnkDBg.bin", language + "/Menu/Bg/pslnkDBg.bin"),
				("pslnkUBg.bin", language + "/Menu/Bg/pslnkUBg.bin"),
				("minigame.bin", language + "/Menu/Tex2D/minigame.bin"),
				("scratch.bin", language + "/Menu/Tex2D/scratch.bin"),
				("ship.bin", language + "/Menu/Tex2D/ship.bin"),
				("title.bin", language + "/Menu/Tex2D/title.bin"),
				("btlChM.bin", language + "/Menu/UI_main/btlChM.bin"),
				("btlCtM.bin", language + "/Menu/UI_main/btlCtM.bin"),
				("btlOpM.bin", language + "/Menu/UI_main/btlOpM.bin"),
				("btlPlM.bin", language + "/Menu/UI_main/btlPlM.bin"),
				("btlRcM.bin", language + "/Menu/UI_main/btlRcM.bin"),
				("btlStM.bin", language + "/Menu/UI_main/btlStM.bin"),
				("cltFishU.bin", language + "/Menu/UI_main/cltFishU.bin"),
				("cltItemU.bin", language + "/Menu/UI_main/cltItemU.bin"),
				("cltStU.bin", language + "/Menu/UI_main/cltStU.bin"),
				("dmHrGsM.bin", language + "/Menu/UI_main/dmHrGsM.bin"),
				("finM.bin", language + "/Menu/UI_main/finM.bin"),
				("fishingM.bin", language + "/Menu/UI_main/fishingM.bin"),
				("grnSwtM.bin", language + "/Menu/UI_main/grnSwtM.bin"),
				("itemShop.bin", language + "/Menu/UI_main/itemShop.bin"),
				("mgM.bin", language + "/Menu/UI_main/mgM.bin"),
				("pauseM.bin", language + "/Menu/UI_main/pauseM.bin"),
				("pauseMVs.bin", language + "/Menu/UI_main/pauseMVs.bin"),
				("pslnkU.bin", language + "/Menu/UI_main/pslnkU.bin"),
				("rplExS.bin", language + "/Menu/UI_main/rplExS.bin"),
				("sgnBd.bin", language + "/Menu/UI_main/sgnBd.bin"),
				("sgnPst.bin", language + "/Menu/UI_main/sgnPst.bin"),
				("skipM.bin", language + "/Menu/UI_main/skipM.bin"),
				("titleM.bin", language + "/Menu/UI_main/titleM.bin"),
				("touchM.bin", language + "/Menu/UI_main/touchM.bin"),
				("UIM.bin", language + "/Menu/UI_main/UIM.bin"),
				("UIMField.bin", language + "/Menu/UI_main/UIMField.bin"),
				("UIMSea.bin", language + "/Menu/UI_main/UIMSea.bin"),
				("LC_Font_m.nftr", "Font/LC_Font_m.nftr"),
				("zeldaDS_15.nftr", "Font/zeldaDS_15.nftr"),
				("zeldaDS_15_btl.nftr", "Font/zeldaDS_15_btl.nftr"),
				("zeldaDS_endL.nftr", "Font/zeldaDS_endL.nftr"),
				("zeldaDS_endS.nftr", "Font/zeldaDS_endS.nftr")
			};

			fontMap = new[]
			{
				("LC_Font_m.nftr", "Font/LC_Font_m.nftr"),
				("zeldaDS_15.nftr", "Font/zeldaDS_15.nftr"),
				("zeldaDS_15_btl.nftr", "Font/zeldaDS_15_btl.nftr"),
				("zeldaDS_endL.nftr", "Font/zeldaDS_endL.nftr"),
				("zeldaDS_endS.nftr", "Font/zeldaDS_endS.nftr")
			};

			if (replaceText && messageFiles.Length > 0)
			{
				log.Report("[TEXT] Replacing message files...");
				foreach (var name in messageFiles)
				{
					string source = Path.Combine(editFolderPath, $"{name}.bmg_out");
					string destInRom = $"/Message/{(name == "train " ? " train_extra " : name)}.bmg";

					await _runner.RunPythonAsync(
						scriptPath: ToolPath("ReimportBMGFile.py"),
						args: new[]
						{
							editedRom,
							source,
							destInRom,
							language,
							editedRom
						},
						workingDir: workingDir,
						log: log,
						ct: ct);

				}

				if (replaceGraphics && graphicMap.Length > 0)
				{
					log.Report("[GRAPHICS] Replacing graphics...");
					string script = ToolPathEither("ReplaceFileInROM.py", "ReplaceFileinROM.py");

					foreach (var (local, romDest) in graphicMap)
					{
						string source = Path.Combine(editFolderPath, local);
						await _runner.RunPythonAsync(
							scriptPath: script,
							args: new[]
							{
								editedRom,
								source,
								romDest
							},
							workingDir: workingDir,
							log: log,
							ct: ct);
					}
				}

				if (replaceFonts && fontMap.Length > 0)
				{
					log.Report("[FONTS] Replacing fonts...");
					string script = ToolPathEither("ReplaceFileInROM.py", "ReplaceFileinROM.py");

					foreach (var (local, romDest) in fontMap)
					{
						string source = Path.Combine(editFolderPath, local);
						await _runner.RunPythonAsync(
							scriptPath: script,
							args: new[]
							{
								editedRom,
								source,
								romDest
							},
							workingDir: workingDir,
							log: log,
							ct: ct);
					}
				}

				log.Report("[DONE] Finished patching ROM.");
			}
		}
	}
}