using Godot;

public partial class StudentNPC : CharacterBody3D, IInteractable
{
    [Export]
    public InteractableArea Area { set; get; } = new();

    public void Interact(Node Interactor)
    {
        GD.Print("Hello world, Kendrick here");
    }
}