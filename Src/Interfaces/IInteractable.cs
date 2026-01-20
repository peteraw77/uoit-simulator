using Godot;

public interface IInteractable
{
    void Interact(Node Interactor);
    
    InteractableArea Area { get; }
}
