using Godot;

public partial class Test3d : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EventBus.Instance.EmitDisplayText
		([
			"Welcome to Ontario Tech.",
			"Your day begins at Charles Hall.",
			"Don't be late for your first class!"
		]);
	}
}
