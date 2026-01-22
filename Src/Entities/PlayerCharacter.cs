using Godot;
using System.Collections.Generic;

public partial class PlayerCharacter : CharacterBody3D
{
	[Export]
	private Area3D InteractionDetector;

	[Export]
	public int Speed { get; set; } = 6;
	
	[Export]
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;

	private HashSet<InteractableArea> _interactables = new();
	private IInteractable _currentInteractable;

	public override void _Ready()
	{
		// exception handling is for nerds
		AnimationPlayer AnimPlayer = GetNode<AnimationPlayer>("Pivot/student/AnimationPlayer");
		AnimPlayer.Play("respirate");
		
		// for stairs
		// ideally would handle this at the level design stage
		FloorMaxAngle = Mathf.DegToRad(70f);

		if (InteractionDetector != null)
		{
			InteractionDetector.AreaEntered += OnAreaEntered;
			InteractionDetector.AreaExited += OnAreaExited;
		}
	}
		
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && _currentInteractable != null)
		{
			_currentInteractable.Interact(this);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = Vector3.Zero;
		
		// check for movement input and adjust direction accordingly
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		
		// update Pivot rotation
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}

		 // set target ground speed
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// if in air, try to fall
		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	private void OnAreaEntered(Area3D OtherArea)
	{
		if (!(OtherArea is InteractableArea))
			return;
		
		_interactables.Add((InteractableArea) OtherArea);

		UpdateCurrentInteractable();
	}

	private void OnAreaExited(Area3D OtherArea)
	{
		if (!(OtherArea is InteractableArea))
			return;
		
		_interactables.Remove((InteractableArea) OtherArea);

		UpdateCurrentInteractable();
	}
	
	private void UpdateCurrentInteractable()
	{
		_currentInteractable = null;
		float bestDistance = float.MaxValue;

		foreach (InteractableArea interactable in _interactables)
		{
			float distance = GlobalPosition.DistanceTo(interactable.GlobalPosition);

			if  (distance < bestDistance && interactable.GetParent() is IInteractable)
			{
				bestDistance = distance;
				
				_currentInteractable = (IInteractable) interactable.GetParent();
			}
		}
	}
}
