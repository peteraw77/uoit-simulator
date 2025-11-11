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
					ShowNextText();
					break;
				case State.Reading:
					// stop text flow and set visibility manually
					Label.VisibleRatio = 1.0f;
					TextboxTween.Stop();
					break;
				case State.Finished:
					if (TextIndex == -1)
					{
						HideTextbox();
						QueueFree();	
					}
					else
					{
						SetState(State.Ready);
						ShowNextText();
					}
					break;
			}
		}
	}

	void HideTextbox(bool IsContainerHidden = true)
	{
		Label.Text = "";

		if (IsContainerHidden)
			Container.Hide();

		SetProcessInput(false);
	}

	public void ShowTextbox()
	{
		Label.Text = DisplayText[TextIndex];
		Container.Show();

		SetProcessInput(true);

		ShowNextText();
	}

	void ShowNextText()
	{
		SetState(State.Reading);

		Label.Text = DisplayText[TextIndex];

		TextboxTween = GetTree().CreateTween();
		TextboxTween.TweenProperty
		(
			Label,
			"visible_characters",
			Label.Text.Length,
			Label.Text.Length * _charReadRate
		).From(0);
		TextboxTween.TweenCallback(Callable.From(() => SetState(State.Finished)));

		TextIndex = TextIndex == DisplayText.Count ? -1 : TextIndex + 1;
	}

	void SetState(State NextState)
	{
		CurrentState = NextState;
	}
}
