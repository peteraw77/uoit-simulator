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
	public Array<string> TextQueue;

	MarginContainer Container;
	Label Label;
	
	Tween TextboxTween;

	private const float _charReadRate = 0.2f;

	State CurrentState;
	
	public Textbox()
	{
		TextQueue = new Array<string>();
	}

	public override void _Ready()
	{
		Container = GetNodeOrNull<MarginContainer>("MarginContainer");
		Label = GetNodeOrNull<Label>("MarginContainer/MarginContainer/HBoxContainer/Label");

		CurrentState = State.Ready;

		HideTextbox();

		// subscribe to textbox event
		EventBus.Instance.DisplayText += ShowTextbox;
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
					SetState(State.Finished);
					break;
				case State.Finished:
					if (TextQueue.Count > 0)
					{
						ShowNextText();
					}
					else
					{
						HideTextbox();
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
		GetTree().Paused = false;
	}

	public void ShowTextbox(Array<string> TextToAdd = null)
	{
		if (TextToAdd != null)
			TextQueue.AddRange(TextToAdd);

		GetTree().Paused = true;
		Container.Show();
		SetProcessInput(true);

		ShowNextText();
	}

	void ShowNextText()
	{
		SetState(State.Reading);

		if (TextQueue.Count > 0)
		{
			Label.Text = TextQueue[0];
			TextQueue.RemoveAt(0);
		}
		else
		{
			Label.Text = "";
		}

		TextboxTween = GetTree().CreateTween();
		TextboxTween.TweenProperty
		(
			Label,
			"visible_characters",
			Label.Text.Length,
			Label.Text.Length * _charReadRate
		).From(0);
		TextboxTween.SetPauseMode(Tween.TweenPauseMode.Process);
		TextboxTween.TweenCallback(Callable.From(() => SetState(State.Finished)));
	}

	void SetState(State NextState)
	{
		CurrentState = NextState;
	}
}
