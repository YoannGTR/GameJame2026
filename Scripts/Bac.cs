using Godot;
using System;

public partial class Bac : Node3D
{
	private int score = 0;

	public override void _Ready()
	{
		var area = GetNode<Area3D>("Area3D");
		area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("Body entered bin: " + body.Name);
		if (body.IsInGroup("pickup"))
		{
			score++;
			GD.Print("Score: " + score);

			body.QueueFree(); // supprime l'objet (optionnel)
		}
	}
}
