using Godot;
using System;

public partial class DoorBoddy : MeshInstance3D
{
	private bool isOpen = false;

	public void ToggleDoor()
	{
		isOpen = !isOpen;

		float angle = isOpen ? 90 : 0;
		RotationDegrees = new Vector3(0, angle, 0);
	}

	public override void _Ready()
	{
		AddToGroup("door");
	}
}
