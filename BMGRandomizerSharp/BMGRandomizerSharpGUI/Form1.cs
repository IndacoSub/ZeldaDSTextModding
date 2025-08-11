using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BMGRandomizerSharpGUI;

public partial class Form1 : Form
{
	private readonly ProcessRunner _runner = new();
	private CancellationTokenSource? _cts;

	public Form1()
	{
		InitializeComponent();
		BuildDynamicInputs();
		ApplyTask(TaskChoice.ExtractText);
	}
}