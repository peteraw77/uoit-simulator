using Godot;
using System;

enum State
{
	Ready,
	Reading,
	Finished
}

public partial class Textbox : CanvasLayer
{
	[Export]
	public String DisplayText = "";

	MarginContainer Container;
	Label Label;

	private const float _charReadRate = 0.1f;

	State CurrentState = State.Ready;

	void _ready()
	{
		Container = GetNodeOrNull<MarginContainer>("MarginContainer");
		Label = GetNodeOrNull<Label>("MarginContainer/MarginContainer/HBoxContainer/Label");

		AddText(DisplayText);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			switch (CurrentState)
			{

			}
		}
	}

	void HideTextbox()
	{
		Label.Text = "";
		Container.Hide();
	}

	void ShowTextbox()
	{
		Label.Text = DisplayText;
		Container.Show();
	}

	void AddText(String NextText)
	{
		DisplayText = NextText;
		ShowTextbox();

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty
		(
			Label,
			"visible_characters",
			DisplayText.Length,
			DisplayText.Length * _charReadRate
		).From(0);
		tween.TweenCallback(Callable.From(() => SetState(State.Finished)));
	}

	void SetState(State NextState)
	{
		CurrentState = NextState;
	}
}
