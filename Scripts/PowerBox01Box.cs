using Godot;
using System;

public partial class PowerBox01Box : MeshInstance3D
{
		public override void _Ready()
	{
		AddToGroup("death");
	}
}
