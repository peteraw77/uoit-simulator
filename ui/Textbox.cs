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

	void _ready()
	{
		Container = GetNodeOrNull<MarginContainer>("MarginContainer");
		Label = GetNodeOrNull<Label>("MarginContainer/MarginContainer/HBoxContainer/Label");

		Tween TextboxTween = GetTree().CreateTween();

		CurrentState = State.Ready;

		TextIndex = 0;

		HideTextbox();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			switch (CurrentState)
			{
				case State.Ready:
					Label.VisibleRatio = 1.0f;
					TextboxTween.Stop(); // why?
					break;
				case State.Reading:
					break;
				case State.Finished:
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
		TextIndex = TextIndex < DisplayText.Count ? TextIndex + 1 : 0;
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
