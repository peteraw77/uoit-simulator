using Godot;
using Godot.Collections;

public partial class EventBus : Node
{
	[Signal]
	public delegate void DisplayTextEventHandler(Array<string> DisplayText);
	
	[Signal]
	public delegate void NamedDisplayTextEventHandler(string Name, Array<string> DisplayText);

	public static EventBus Instance;

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitDisplayText(Array<string> DisplayText)
	{
		EmitSignal(SignalName.DisplayText, DisplayText);
	}

	public void EmitNamedDisplayText(string Name, Array<string> DisplayText)
	{
		EmitSignal(SignalName.NamedDisplayText, [Name, DisplayText]);
	}
}
