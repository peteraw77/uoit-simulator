using Godot;

public interface IInteractable
{
    void Interact(Node Interactor);
    
    InteractableArea Area { set; get; }
}
