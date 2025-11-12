using Godot;
using System;

public partial class Test3d : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EventBus.Instance.EmitDisplayText(["Hi, my name is Bobbe"]);      
	}
}
