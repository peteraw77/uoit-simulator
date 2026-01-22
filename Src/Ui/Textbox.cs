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

	private TextboxRoot Root;

	Label Label;
	
	Tween TextboxTween;

	private const float _charReadRate = .04f;

	State CurrentState;
	
	public Textbox()
	{
		TextQueue = new Array<string>();
	}

	public override void _Ready()
	{
		Root = GetNodeOrNull<TextboxRoot>("Root");
		Label = GetNodeOrNull<Label>("Root/MarginContainer/HBoxContainer/Label");

		CurrentState = State.Ready;

		HideTextbox();
		
		// subscribe to controller input
		Root.AdvanceRequested += Advance;

		// subscribe to textbox event
		EventBus.Instance.DisplayText += ShowTextbox;
	}

	public void Advance()
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

	void HideTextbox(bool IsContainerHidden = true)
	{
		Label.Text = "";

		if (IsContainerHidden)
			Root.Hide();

		GetTree().Paused = false;
	}

	public void ShowTextbox(Array<string> TextToAdd = null)
	{
		if (TextToAdd != null)
			TextQueue.AddRange(TextToAdd);

		GetTree().Paused = true;
		Root.Show();
		
		Root.Activate();

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
