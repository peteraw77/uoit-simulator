using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

enum State
{
	Ready,
	Reading,
	Finished
}

public partial class Textbox : CanvasLayer
{
	[Export]
	public Array<string> DisplayText;

	MarginContainer Container;
	Label Label;
	
	Tween TextboxTween;

	private const float _charReadRate = 0.1f;

	State CurrentState;

	int TextIndex;
	
	public Textbox()
    {
		DisplayText = new Array<string>();

		TextIndex = 0;
    }

	public override void _Ready()
	{
		Container = GetNodeOrNull<MarginContainer>("MarginContainer");
		Label = GetNodeOrNull<Label>("MarginContainer/MarginContainer/HBoxContainer/Label");

		Tween TextboxTween = GetTree().CreateTween();

		CurrentState = State.Ready;

		HideTextbox();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			switch (CurrentState)
			{
				case State.Ready:
					if (TextIndex > -1)
						ShowNextText();
					break;
				case State.Reading:
					// stop text flow and set visibility manually
					Label.VisibleRatio = 1.0f;
					TextboxTween.Stop();
					break;
				case State.Finished:
					SetState(State.Ready);
					HideTextbox();
					break;
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
		Label.Text = DisplayText[TextIndex];
		Container.Show();
	}

	void ShowNextText()
	{
		TextIndex = TextIndex < DisplayText.Count ? TextIndex + 1 : -1;
		ShowTextbox();

		string CurrentText = DisplayText[TextIndex];

		TextboxTween.TweenProperty
		(
			Label,
			"visible_characters",
			CurrentText.Length,
			CurrentText.Length * _charReadRate
		).From(0);
		TextboxTween.TweenCallback(Callable.From(() => SetState(State.Finished)));
	}

	void SetState(State NextState)
	{
		CurrentState = NextState;
	}
}
