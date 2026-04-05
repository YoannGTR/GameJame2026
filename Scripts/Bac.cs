using Godot;
using System;

public partial class Bac : Node3D
{
	private int score = 0;

	public override void _Ready()
	{
		var area = GetNode<Area3D>("Area3D");
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;

		AddToGroup("bac");
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("Body entered bin: " + body.Name);
		if (body.IsInGroup("toMoveOnBac"))
		{
			score++;
			GD.Print("Score: " + score);

			// body.QueueFree(); // supprime l'objet (optionnel)
		}
	}

	private void OnBodyExited(Node body)
	{
		GD.Print("Body exited bin: " + body.Name);
		if (body.IsInGroup("toMoveOnBac"))
		{
			score--;
			GD.Print("Score: " + score);

		}
	}
}
