using Godot;

public partial class StudentNPC : CharacterBody3D, IInteractable
{
    [Export]
    public InteractableArea Area { set; get; }
    
    public override void _Ready()
    {
		// exception handling is for nerds
		AnimationPlayer AnimPlayer = GetNode<AnimationPlayer>("Pivot/student/AnimationPlayer");
		AnimPlayer.Play("respirate");
    }

    public void Interact(Node Interactor)
    {
        GD.Print("Hello world, Kendrick here");
    }
}