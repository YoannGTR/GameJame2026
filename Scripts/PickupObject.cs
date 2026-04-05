using Godot;
using System;

public partial class PickupObject : RigidBody3D
{
	public override void _Ready()
	{
		AddToGroup("pickup");
		AddToGroup("toMoveOnBac");
	}

	// verify if the object is outside of the map and reset its position if it is
	public override void _Process(double delta)
	{		
		if (GlobalPosition.Y < -10)
		{
			GlobalPosition = new Vector3(0, 15, 0);
			LinearVelocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
		}
	}
}
