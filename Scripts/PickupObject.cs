using Godot;
using System;

public partial class PickupObject : RigidBody3D
{
	public override void _Ready()
	{
		AddToGroup("pickup");
		AddToGroup("toMoveOnBac");
		GravityScale = 4.0f; // rend l'objet plus lourd pour éviter qu'il ne vole trop loin
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
