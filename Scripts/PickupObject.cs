using Godot;
using System;

public partial class PickupObject : RigidBody3D
{
	public override void _Ready()
	{
		AddToGroup("pickup");
	}
}
