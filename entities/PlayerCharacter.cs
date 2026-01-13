using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerCharacter : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;
	
	[Export]
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _Ready()
	{
		// exception handling is for nerds
		AnimationPlayer AnimPlayer = GetNode<AnimationPlayer>("Pivot/student/AnimationPlayer");
		AnimPlayer.Play("respirate");
		
		// for stairs
		// ideally would handle this at the level design stage
		FloorMaxAngle = Mathf.DegToRad(70f);
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
}
