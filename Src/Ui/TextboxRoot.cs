using Godot;
public partial class TextboxRoot : MarginContainer
{
    private Textbox Textbox;

    public override void _Ready()
    {
        Textbox = GetParent<Textbox>();

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
            Textbox.Advance();
            AcceptEvent();
        }
    }
}
