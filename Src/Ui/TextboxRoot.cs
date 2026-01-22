using Godot;
public partial class TextboxRoot : MarginContainer
{
	[Signal]
    public delegate void AdvanceRequestedEventHandler();	

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.WhenPaused;
        MouseFilter = MouseFilterEnum.Stop;
        FocusMode = FocusModeEnum.All;
    }
 
    public void Activate()
    {
        GrabFocus();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            // to be consumed by textbox
            EmitSignal(SignalName.AdvanceRequested);

            AcceptEvent();
        }
    }
}
