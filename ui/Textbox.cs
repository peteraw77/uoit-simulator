using Godot;
using System;

public partial class Textbox : CanvasLayer
{
	[Export]
	public String DisplayText = "";

	MarginContainer Container;
	Label Label;

	private const float _charReadRate = 0.1f;

	void _ready()
	{
		Container = GetNodeOrNull<MarginContainer>("MarginContainer");
		Label = GetNodeOrNull<Label>("MarginContainer/MarginContainer/HBoxContainer/Label");

		AddText(DisplayText);
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
	}
}
