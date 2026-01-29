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
	private Container TextContainer;
	private Container NameContainer;

	Label TextLabel;
	Label NameLabel;
	
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
		TextContainer = GetNodeOrNull<Container>("Root/TextContainer");
		NameContainer = GetNodeOrNull<Container>("Root/NameContainer");
		TextLabel = GetNodeOrNull<Label>("Root/TextContainer/MarginContainer/HBoxContainer/Label");
		NameLabel = GetNodeOrNull<Label>("Root/NameContainer/MarginContainer/Label");

		CurrentState = State.Ready;

		HideTextbox();
		
		// subscribe to controller input
		Root.AdvanceRequested += Advance;

		// subscribe to textbox event
		EventBus.Instance.DisplayText += ShowTextbox;
		EventBus.Instance.NamedDisplayText += ShowNamedTextbox;
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
				TextLabel.VisibleRatio = 1.0f;
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
		TextLabel.Text = "";

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
		
		// hide name
		NameContainer.Hide();

		ShowNextText();
	}

	public void ShowNamedTextbox(string Name, Array<string> TextToAdd = null)
	{
		ShowTextbox(TextToAdd);
	
		// show name
		NameLabel.Text = Name;
		NameContainer.Show();
	}

	void ShowNextText()
	{
		SetState(State.Reading);

		if (TextQueue.Count > 0)
		{
			TextLabel.Text = TextQueue[0];
			TextQueue.RemoveAt(0);
		}
		else
		{
			TextLabel.Text = "";
		}

		TextboxTween = GetTree().CreateTween();
		TextboxTween.TweenProperty
		(
			TextLabel,
			"visible_characters",
			TextLabel.Text.Length,
			TextLabel.Text.Length * _charReadRate
		).From(0);
		TextboxTween.SetPauseMode(Tween.TweenPauseMode.Process);
		TextboxTween.TweenCallback(Callable.From(() => SetState(State.Finished)));
	}

	void SetState(State NextState)
	{
		CurrentState = NextState;
	}
}
