using Godot;
using System;

public partial class Plane024 : MeshInstance3D
{
		public override void _Ready()
	{
		var area = GetNode<Area3D>("Area3D");
		area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Player")){
			GetNode<CanvasLayer>("/root/niveau_1/Defaite").Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			GetTree().Paused = true;
		}
	}
}
